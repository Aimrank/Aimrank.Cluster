using Aimrank.Cluster.Core.Validation;
using MediatR;
using System;

namespace Aimrank.Cluster.Core.Commands.DeleteAndStopServer
{
    public class DeleteAndStopServerCommand : IRequest
    {
        [NotEmpty]
        public Guid Id { get; set; }
    }
}