using Aimrank.Cluster.Core.Exceptions;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.DeleteSteamToken
{
    internal class DeleteSteamTokenCommandHandler : IRequestHandler<DeleteSteamTokenCommand>
    {
        private readonly ISteamTokenRepository _steamTokenRepository;

        public DeleteSteamTokenCommandHandler(ISteamTokenRepository steamTokenRepository)
        {
            _steamTokenRepository = steamTokenRepository;
        }

        public async Task<Unit> Handle(DeleteSteamTokenCommand request, CancellationToken cancellationToken)
        {
            var steamToken = await _steamTokenRepository.GetAsync(request.Token);
            if (steamToken.Server is not null)
            {
                throw new ClusterException(
                    $"Cannot delete steam token '{request.Token}' because it's used by server '{steamToken.Server.Id}'.");
            }
            
            _steamTokenRepository.Delete(steamToken);
            
            return Unit.Value;
        }
    }
}