using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Exceptions;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.CreateServers
{
    internal class CreateServersCommandHandler : IRequestHandler<CreateServersCommand>
    {
        private readonly ISteamTokenRepository _steamTokenRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IPodRepository _podRepository;

        public CreateServersCommandHandler(
            ISteamTokenRepository steamTokenRepository,
            IServerRepository serverRepository,
            IPodRepository podRepository)
        {
            _steamTokenRepository = steamTokenRepository;
            _serverRepository = serverRepository;
            _podRepository = podRepository;
        }

        public async Task<Unit> Handle(CreateServersCommand request, CancellationToken cancellationToken)
        {
            var matches = request.Ids.ToList();
            
            var tokens = (await _steamTokenRepository.BrowseUnusedAsync(matches.Count)).ToList();
            if (tokens.Count != matches.Count)
            {
                throw new ClusterException("Not enough steam tokens.");
            }

            var entries = (await _podRepository.BrowseAsync())
                .Select(p => new PodDto
                {
                    Pod = p,
                    AvailableServers = p.MaxServers - p.Servers.Count
                })
                .ToList();

            for (var i = 0; i < matches.Count; i++)
            {
                var entry = entries.FirstOrDefault(e => e.AvailableServers > 0);
                if (entry is null)
                {
                    throw new ClusterException("No server available.");
                }

                var server = new Server
                {
                    Id = matches[i],
                    Pod = entry.Pod,
                    SteamToken = tokens[i]
                };

                entry.AvailableServers--;

                _serverRepository.Add(server);
            }
            
            return Unit.Value;
        }

        private class PodDto
        {
            public Pod Pod { get; set; }
            public int AvailableServers { get; set; }
        }
    }
}