using Aimrank.Cluster.Core.Validation;
using MediatR;
using System;

namespace Aimrank.Cluster.Core.Commands.DeleteServer
{
    public class DeleteServerCommand : IRequest
    {
        [NotEmpty]
        public Guid Id { get; set; }
    }
}