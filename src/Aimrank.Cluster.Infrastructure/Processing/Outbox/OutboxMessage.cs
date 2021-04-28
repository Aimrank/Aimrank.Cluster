using System;

namespace Aimrank.Cluster.Infrastructure.Processing.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; }
        public string Type { get; }
        public string Data { get; }
        public DateTime OccurredOn { get; }
        public DateTime? ProcessedDate { get; set; }
        
        private OutboxMessage() {}

        public OutboxMessage(Guid id, string type, string data, DateTime occurredOn)
        {
            Id = id;
            Type = type;
            Data = data;
            OccurredOn = occurredOn;
        }
    }
}