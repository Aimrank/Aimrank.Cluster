using Aimrank.Cluster.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Events.External.MatchCanceled
{
    internal class MatchCanceledEventHandler : IEventHandler<MatchCanceledEvent>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MatchCanceledEventHandler(IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverRepository = serverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MatchCanceledEvent notification, CancellationToken cancellationToken)
        {
            var server = await _serverRepository.GetByIdAsync(notification.MatchId);
            
            _serverRepository.Delete(server);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}