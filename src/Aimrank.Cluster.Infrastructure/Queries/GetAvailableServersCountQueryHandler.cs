using Aimrank.Cluster.Core.Queries.GetAvailableServersCount;
using Aimrank.Cluster.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Infrastructure.Queries
{
    internal class GetAvailableServersCountQueryHandler : IRequestHandler<GetAvailableServersCountQuery, int>
    {
        private readonly ClusterContext _context;

        public GetAvailableServersCountQueryHandler(ClusterContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetAvailableServersCountQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Pods
                .Include(p => p.Servers)
                .Select(p => new
                {
                    p.MaxServers,
                    p.Servers.Count
                })
                .ToListAsync(cancellationToken);

            return result.Sum(p => p.MaxServers - p.Count);
        }
    }
}