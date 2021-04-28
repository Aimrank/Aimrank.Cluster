using Microsoft.Extensions.Options;
using System.Reflection;

namespace Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ
{
    internal class RabbitMQRoutingKeyFactory
    {
        private readonly RabbitMQSettings _rabbitMqSettings;

        public RabbitMQRoutingKeyFactory(IOptions<RabbitMQSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
        }

        public string Create(MemberInfo type, string serviceName = null)
            => $"{serviceName ?? _rabbitMqSettings.ServiceName}.{type.Name}";
    }
}