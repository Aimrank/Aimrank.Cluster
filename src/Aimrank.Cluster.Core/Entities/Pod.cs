using System.Collections.Generic;

namespace Aimrank.Cluster.Core.Entities
{
    public class Pod
    {
        public string IpAddress { get; set; }
        public int MaxServers { get; set; }
        public ICollection<Server> Servers { get; set; }
    }
}