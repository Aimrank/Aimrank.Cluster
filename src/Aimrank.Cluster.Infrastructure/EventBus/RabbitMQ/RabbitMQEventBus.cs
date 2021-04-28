using Aimrank.Cluster.Core.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ
{
    internal class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly RabbitMQSettings _rabbitMqSettings;
        private readonly RabbitMQEventSerializer _eventSerializer;
        private readonly RabbitMQRoutingKeyFactory _routingKeyFactory;
        private readonly IBasicProperties _basicProperties;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQEventBus(
            IOptions<RabbitMQSettings> rabbitMqSettings,
            RabbitMQEventSerializer eventSerializer,
            RabbitMQRoutingKeyFactory routingKeyFactory)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _eventSerializer = eventSerializer;
            _routingKeyFactory = routingKeyFactory;
            
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSettings.HostName,
                Port = _rabbitMqSettings.Port,
                UserName = _rabbitMqSettings.Username,
                Password = _rabbitMqSettings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_rabbitMqSettings.ExchangeName, "direct", true, false, null);
            _basicProperties = _channel.CreateBasicProperties();
            _basicProperties.Persistent = true;
        }

        public void Publish(IEvent @event)
            => _channel.BasicPublish(
                _rabbitMqSettings.ExchangeName,
                _routingKeyFactory.Create(@event.GetType()),
                _basicProperties,
                _eventSerializer.Serialize(@event));
        
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Dispose();
        }
    }
}