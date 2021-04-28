using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}