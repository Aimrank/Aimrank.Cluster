using System;

namespace Aimrank.Cluster.Core.Entities
{
    public class Server
    {
        public Guid Id { get; set; }
        public bool IsAccepted { get; set; }
        public SteamToken SteamToken { get; set; }
        public Pod Pod { get; set; }
    }
}