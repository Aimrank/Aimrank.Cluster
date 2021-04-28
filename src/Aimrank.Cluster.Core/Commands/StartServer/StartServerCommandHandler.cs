using Aimrank.Cluster.Core.Clients;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.StartServer
{
    internal class StartServerCommandHandler : IRequestHandler<StartServerCommand, string>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IPodClient _podClient;

        public StartServerCommandHandler(IServerRepository serverRepository, IPodClient podClient)
        {
            _serverRepository = serverRepository;
            _podClient = podClient;
        }

        public async Task<string> Handle(StartServerCommand request, CancellationToken cancellationToken)
        {
            var server = await _serverRepository.GetByIdAsync(request.Id);

            server.IsAccepted = true;

            return await _podClient.StartServerAsync(server, request.Map, request.Whitelist);
        }
    }
}