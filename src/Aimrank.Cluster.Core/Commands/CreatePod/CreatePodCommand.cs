using Aimrank.Cluster.Core.Validation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Aimrank.Cluster.Core.Commands.CreatePod
{
    public class CreatePodCommand : IRequest
    {
        [NotEmpty]
        public string IpAddress { get; set; }
        
        [Range(1, int.MaxValue)]
        public int MaxServers { get; set; }
    }
}