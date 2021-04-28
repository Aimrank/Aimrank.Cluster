using System;

namespace Aimrank.Cluster.Infrastructure.Processing.Inbox
{
    public class InboxMessage
    {
        public Guid Id { get; }
        public string Type { get; }
        public string Data { get; }
        public DateTime OccurredOn { get; }
        public DateTime? ProcessedDate { get; set; }
        
        private InboxMessage() {}

        public InboxMessage(Guid id, string type, string data, DateTime occurredOn)
        {
            Id = id;
            Type = type;
            Data = data;
            OccurredOn = occurredOn;
        }
    }
}