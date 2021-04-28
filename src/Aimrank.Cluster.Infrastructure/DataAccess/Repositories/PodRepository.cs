using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Repositories
{
    internal sealed class PodRepository : IPodRepository
    {
        private readonly ClusterContext _context;

        public PodRepository(ClusterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pod>> BrowseAsync()
            => await _context.Pods.Include(p => p.Servers).ToListAsync();

        public Task<Pod> GetOptionalAsync(string ipAddress)
            => _context.Pods.FirstOrDefaultAsync(p => p.IpAddress == ipAddress);

        public void Add(Pod pod) => _context.Add(pod);

        public void DeleteRange(IEnumerable<Pod> pods) => _context.RemoveRange(pods);
    }
}