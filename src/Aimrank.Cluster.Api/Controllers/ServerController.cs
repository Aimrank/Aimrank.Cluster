using Aimrank.Cluster.Core.Commands.CreateServers;
using Aimrank.Cluster.Core.Commands.DeleteServer;
using Aimrank.Cluster.Core.Commands.StartServer;
using Aimrank.Cluster.Core.Queries.GetAvailableServersCount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Aimrank.Cluster.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableServersCount()
        {
            var count = await _mediator.Send(new GetAvailableServersCountQuery());
            return Ok(new {Count = count});
        }

        [HttpPost]
        public async Task<IActionResult> CreateServers(CreateServersCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartServer(StartServerCommand command)
        {
            var address = await _mediator.Send(command);
            return Ok(new {Address = address});
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteServer(Guid id)
        {
            await _mediator.Send(new DeleteServerCommand {Id = id});
            return NoContent();
        }
    }
}