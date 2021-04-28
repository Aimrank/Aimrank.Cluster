using MediatR;

namespace Aimrank.Cluster.Core.Events
{
    public interface IEventHandler<in T> : INotificationHandler<T> where T : class, IEvent
    {
    }
}