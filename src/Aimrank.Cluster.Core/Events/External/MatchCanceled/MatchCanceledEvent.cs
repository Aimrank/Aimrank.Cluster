using System;

namespace Aimrank.Cluster.Core.Events.External.MatchCanceled
{
    [Event("Aimrank.Pod")]
    public class MatchCanceledEvent : IEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
        public Guid MatchId { get; }

        public MatchCanceledEvent(Guid id, DateTime occurredOn, Guid matchId)
        {
            Id = id;
            OccurredOn = occurredOn;
            MatchId = matchId;
        }
    }
}