using Aimrank.Cluster.Core.Commands.CreatePod;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePod(CreatePodCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}