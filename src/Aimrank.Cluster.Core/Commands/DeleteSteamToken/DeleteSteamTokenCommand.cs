using Aimrank.Cluster.Core.Validation;
using MediatR;

namespace Aimrank.Cluster.Core.Commands.DeleteSteamToken
{
    public class DeleteSteamTokenCommand : IRequest
    {
        [NotEmpty]
        public string Token { get; set; }
    }
}