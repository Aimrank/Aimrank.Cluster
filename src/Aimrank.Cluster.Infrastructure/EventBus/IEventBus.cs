using Aimrank.Cluster.Core.Events;

namespace Aimrank.Cluster.Infrastructure.EventBus
{
    internal interface IEventBus
    {
        void Publish(IEvent @event);
    }
}