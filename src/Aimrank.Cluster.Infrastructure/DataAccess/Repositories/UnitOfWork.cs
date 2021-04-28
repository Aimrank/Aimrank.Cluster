using Aimrank.Cluster.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ClusterContext _context;

        public UnitOfWork(ClusterContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}