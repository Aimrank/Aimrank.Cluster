using Aimrank.Cluster.Core.Events;
using System.Text.Json;
using System.Text;
using System;

namespace Aimrank.Cluster.Infrastructure.EventBus.RabbitMQ
{
    internal class RabbitMQEventSerializer
    {
        public byte[] Serialize(IEvent @event)
            => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event, @event.GetType()));

        public IEvent Deserialize(byte[] data, Type type)
        {
            var text = Encoding.UTF8.GetString(data);
            return (IEvent) JsonSerializer.Deserialize(text, type);
        }
    }
}