namespace Aimrank.Cluster.Core.Events
{
    public interface IEventsDispatcher
    {
        void Dispatch(IEvent @event);
    }
}