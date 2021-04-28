using Aimrank.Cluster.Core.Events;
using System.Collections.Generic;
using System;

namespace Aimrank.Cluster.Core.Commands.RemoveInactivePods
{
    public class ServersDeletedEvent : EventBase
    {
        public IEnumerable<Guid> Ids { get; }

        public ServersDeletedEvent(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}