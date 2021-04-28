using Aimrank.Cluster.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Core.Clients
{
    public interface IPodClient
    {
        Task<IEnumerable<Pod>> GetInactivePodsAsync();
        Task StopServerAsync(Server server);
        Task<string> StartServerAsync(Server server, string map, IEnumerable<string> whitelist);
    }
}