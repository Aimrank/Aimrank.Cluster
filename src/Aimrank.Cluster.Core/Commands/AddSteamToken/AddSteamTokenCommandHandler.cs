using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Exceptions;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.AddSteamToken
{
    internal class AddSteamTokenCommandHandler : IRequestHandler<AddSteamTokenCommand>
    {
        private readonly ISteamTokenRepository _steamTokenRepository;

        public AddSteamTokenCommandHandler(ISteamTokenRepository steamTokenRepository)
        {
            _steamTokenRepository = steamTokenRepository;
        }

        public async Task<Unit> Handle(AddSteamTokenCommand request, CancellationToken cancellationToken)
        {
            var steamToken = await _steamTokenRepository.GetOptionalAsync(request.Token);
            if (steamToken is not null)
            {
                throw new ClusterException($"Steam token '{request.Token}' already exists.");
            }

            steamToken = new SteamToken {Token = request.Token};

            _steamTokenRepository.Add(steamToken);
            
            return Unit.Value;
        }
    }
}