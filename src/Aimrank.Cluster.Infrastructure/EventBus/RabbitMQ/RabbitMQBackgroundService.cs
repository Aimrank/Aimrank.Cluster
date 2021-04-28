using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.Processing.Inbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ
{
    internal class RabbitMQBackgroundService : BackgroundService
    {
        private readonly Dictionary<string, Type> _events;
        private readonly RabbitMQSettings _rabbitMqSettings;
        private readonly RabbitMQEventSerializer _eventSerializer;
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IBasicProperties _basicProperties;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQBackgroundService(
            IOptions<RabbitMQSettings> rabbitMqSettings,
            RabbitMQEventSerializer eventSerializer,
            RabbitMQRoutingKeyFactory routingKeyFactory,
            ILogger<RabbitMQBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _eventSerializer = eventSerializer;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            
            _events = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<EventAttribute>() is not null)
                .ToDictionary(t => routingKeyFactory.Create(t, t.GetCustomAttribute<EventAttribute>().Service));
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
        public void Configure() { _connection = CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_rabbitMqSettings.ExchangeName, "direct", true, false, null);
            _channel.QueueDeclare(_rabbitMqSettings.ServiceName, true, false, false, null);
            _basicProperties = _channel.CreateBasicProperties();
            _basicProperties.Persistent = true;

            foreach (var (routingKey, _) in _events)
            {
                _channel.QueueBind(_rabbitMqSettings.ServiceName, _rabbitMqSettings.ExchangeName, routingKey);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (_channel is null)
            {
                await Task.Delay(1000, stoppingToken);
            }
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            
            consumer.Received += async (_, ea) =>
            {
                var type = _events.GetValueOrDefault(ea.RoutingKey);
                if (type is null)
                {
                    _logger.LogWarning($"No event was mapped for routing key '{ea.RoutingKey}'.");
                    return;
                }

                await AddEventToInboxAsync(_eventSerializer.Deserialize(ea.Body.ToArray(), type));
                
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            
            _channel.BasicConsume(_rabbitMqSettings.ServiceName, false, consumer: consumer);
        }

        private async Task AddEventToInboxAsync(IEvent @event)
        {
            var message = new InboxMessage(
                @event.Id,
                @event.GetType().FullName,
                JsonSerializer.Serialize(@event, @event.GetType()),
                @event.OccurredOn);

            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ClusterContext>();
            await context.InboxMessages.AddAsync(message);
            await context.SaveChangesAsync();
        }

        private IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSettings.HostName,
                Port = _rabbitMqSettings.Port,
                UserName = _rabbitMqSettings.Username,
                Password = _rabbitMqSettings.Password,
                DispatchConsumersAsync = true
            };

            var attempts = 0;

            while (attempts <= _rabbitMqSettings.MaxRetries)
            {
                try
                {
                    return factory.CreateConnection();
                }
                catch (BrokerUnreachableException)
                {
                    _logger.LogError("Failed to connect to RabbitMQ. Retrying in 10 seconds.");

                    attempts++;
                    
                    if (attempts <= _rabbitMqSettings.MaxRetries)
                    {
                        Thread.Sleep(10000);
                    }
                }
            }
            
            throw new Exception("Failed to connect to RabbitMQ.");
        }
    }
}