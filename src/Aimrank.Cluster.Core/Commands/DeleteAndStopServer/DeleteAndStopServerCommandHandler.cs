using Aimrank.Cluster.Core.Clients;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.DeleteAndStopServer
{
    internal class DeleteAndStopServerCommandHandler : IRequestHandler<DeleteAndStopServerCommand>
    {
        private readonly IServerRepository _serverRepository;
        private readonly IPodClient _podClient;

        public DeleteAndStopServerCommandHandler(IServerRepository serverRepository, IPodClient podClient)
        {
            _serverRepository = serverRepository;
            _podClient = podClient;
        }

        public async Task<Unit> Handle(DeleteAndStopServerCommand request, CancellationToken cancellationToken)
        {
            var server = await _serverRepository.GetByIdAsync(request.Id);

            if (server.IsAccepted)
            {
                await _podClient.StopServerAsync(server);
            }
            
            _serverRepository.Delete(server);

            return Unit.Value;
        }
    }
}