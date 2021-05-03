using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.Processing.Inbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
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
        private readonly IBasicProperties _basicProperties;
        private readonly IConnection _connection;
        private readonly IModel _channel;

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
            
            _connection = CreateConnection();
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

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

            return Task.CompletedTask;
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
            
            return factory.CreateConnection();
        }
    }
}