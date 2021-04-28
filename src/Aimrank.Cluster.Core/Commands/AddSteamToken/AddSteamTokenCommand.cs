using Aimrank.Cluster.Core.Validation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Aimrank.Cluster.Core.Commands.AddSteamToken
{
    public class AddSteamTokenCommand : IRequest
    {
        [NotEmpty]
        [RegularExpression("[0-9A-Z]{32}", ErrorMessage = "Invalid token format.")]
        public string Token { get; set; }
    }
}