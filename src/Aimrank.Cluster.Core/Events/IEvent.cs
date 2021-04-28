using System;
using MediatR;

namespace Aimrank.Cluster.Core.Events
{
    public interface IEvent : INotification
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
    }
}