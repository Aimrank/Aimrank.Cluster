using Aimrank.Cluster.Core.Commands.AddSteamToken;
using Aimrank.Cluster.Core.Commands.DeleteSteamToken;
using Aimrank.Cluster.Core.Queries.GetSteamTokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Api.Controllers
{
    [ApiController]
    [Route("steam-token")]
    public class SteamTokenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SteamTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSteamTokens()
            => Ok(await _mediator.Send(new GetSteamTokensQuery()));

        [HttpPost]
        public async Task<IActionResult> AddSteamToken(AddSteamTokenCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{token}")]
        public async Task<IActionResult> DeleteSteamToken(string token)
        {
            await _mediator.Send(new DeleteSteamTokenCommand {Token = token});
            return NoContent();
        }
    }
}