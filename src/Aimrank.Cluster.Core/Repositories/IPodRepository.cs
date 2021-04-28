using Aimrank.Cluster.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Core.Repositories
{
    public interface IPodRepository
    {
        Task<IEnumerable<Pod>> BrowseAsync();
        Task<Pod> GetOptionalAsync(string ipAddress);
        void Add(Pod pod);
        void DeleteRange(IEnumerable<Pod> pods);
    }
}