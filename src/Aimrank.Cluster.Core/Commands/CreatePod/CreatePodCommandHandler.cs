using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Core.Commands.CreatePod
{
    internal class CreatePodCommandHandler : IRequestHandler<CreatePodCommand>
    {
        private readonly IPodRepository _podRepository;

        public CreatePodCommandHandler(IPodRepository podRepository)
        {
            _podRepository = podRepository;
        }

        public async Task<Unit> Handle(CreatePodCommand request, CancellationToken cancellationToken)
        {
            var pod = await _podRepository.GetOptionalAsync(request.IpAddress);
            if (pod is not null)
            {
                return Unit.Value;
            }

            pod = new Pod
            {
                IpAddress = request.IpAddress,
                MaxServers = request.MaxServers
            };

            _podRepository.Add(pod);
            
            return Unit.Value;
        }
    }
}