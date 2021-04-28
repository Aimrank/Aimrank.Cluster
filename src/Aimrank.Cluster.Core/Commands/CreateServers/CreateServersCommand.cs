using Aimrank.Cluster.Core.Validation;
using MediatR;
using System.Collections.Generic;
using System;

namespace Aimrank.Cluster.Core.Commands.CreateServers
{
    public class CreateServersCommand : IRequest
    {
        [NotEmpty]
        public IEnumerable<Guid> Ids { get; set; }
    }
}