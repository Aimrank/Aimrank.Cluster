using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using System.Text.Json;

namespace Aimrank.Cluster.Infrastructure.Processing.Outbox
{
    internal class OutboxEventsDispatcher : IEventsDispatcher
    {
        private readonly ClusterContext _context;

        public OutboxEventsDispatcher(ClusterContext context)
        {
            _context = context;
        }

        public void Dispatch(IEvent @event)
            => _context.OutboxMessages.Add(new OutboxMessage(
                @event.Id,
                @event.GetType().FullName,
                JsonSerializer.Serialize(@event, @event.GetType()),
                @event.OccurredOn));
    }
}