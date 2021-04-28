using System;

namespace Aimrank.Cluster.Core.Events.External.MatchFinishedEvent
{
    [Event("Aimrank.Pod")]
    public class MatchFinishedEvent : IEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
        public Guid MatchId { get; }

        public MatchFinishedEvent(Guid id, DateTime occurredOn, Guid matchId)
        {
            Id = id;
            OccurredOn = occurredOn;
            MatchId = matchId;
        }
    }
}