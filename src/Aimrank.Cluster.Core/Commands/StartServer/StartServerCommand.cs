using Aimrank.Cluster.Core.Validation;
using MediatR;
using System.Collections.Generic;
using System;

namespace Aimrank.Cluster.Core.Commands.StartServer
{
    public class StartServerCommand : IRequest<string>
    {
        [NotEmpty]
        public Guid Id { get; set; }
        
        [NotEmpty]
        public string Map { get; set; }
        
        [NotEmpty]
        public IEnumerable<string> Whitelist { get; set; }
    }
}