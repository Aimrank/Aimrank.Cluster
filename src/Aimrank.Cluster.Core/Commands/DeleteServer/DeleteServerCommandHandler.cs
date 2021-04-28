using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.DeleteServer
{
    internal class DeleteServerCommandHandler : IRequestHandler<DeleteServerCommand>
    {
        private readonly IServerRepository _serverRepository;

        public DeleteServerCommandHandler(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<Unit> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            var server = await _serverRepository.GetByIdAsync(request.Id);
            
            _serverRepository.Delete(server);
            
            return Unit.Value;
        }
    }
}