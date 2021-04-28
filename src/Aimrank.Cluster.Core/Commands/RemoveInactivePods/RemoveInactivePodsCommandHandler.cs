using Aimrank.Cluster.Core.Clients;
using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.RemoveInactivePods
{
    internal class RemoveInactivePodsCommandHandler : IRequestHandler<RemoveInactivePodsCommand>
    {
        private readonly IPodClient _podClient;
        private readonly IPodRepository _podRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IEventsDispatcher _eventsDispatcher;

        public RemoveInactivePodsCommandHandler(
            IPodClient podClient,
            IPodRepository podRepository,
            IServerRepository serverRepository,
            IEventsDispatcher eventsDispatcher)
        {
            _podClient = podClient;
            _podRepository = podRepository;
            _serverRepository = serverRepository;
            _eventsDispatcher = eventsDispatcher;
        }

        public async Task<Unit> Handle(RemoveInactivePodsCommand request, CancellationToken cancellationToken)
        {
            var inactivePods = (await _podClient.GetInactivePodsAsync()).ToList();
            var inactivePodsIp = inactivePods.Select(p => p.IpAddress);

            var inactiveServers = (await _serverRepository.BrowseByIpAddressesAsync(inactivePodsIp)).ToList();
            if (inactiveServers.Any())
            {
                _serverRepository.DeleteRange(inactiveServers);
                _podRepository.DeleteRange(inactivePods);
                _eventsDispatcher.Dispatch(new ServersDeletedEvent(inactiveServers.Select(s => s.Id)));
            }

            return Unit.Value;
        }
    }
}