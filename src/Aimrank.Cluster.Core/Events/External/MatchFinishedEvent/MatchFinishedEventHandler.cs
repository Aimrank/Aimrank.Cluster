using System.Threading;
using System.Threading.Tasks;
using Aimrank.Cluster.Core.Repositories;

namespace Aimrank.Cluster.Core.Events.External.MatchFinishedEvent
{
    internal class MatchFinishedEventHandler : IEventHandler<MatchFinishedEvent>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MatchFinishedEventHandler(IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverRepository = serverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MatchFinishedEvent notification, CancellationToken cancellationToken)
        {
            var server = await _serverRepository.GetByIdAsync(notification.MatchId);
            
            _serverRepository.Delete(server);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}