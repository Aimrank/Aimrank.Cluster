using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Aimrank.Cluster.Infrastructure.Processing.Inbox
{
    internal class ProcessInboxCommandHandler : IRequestHandler<ProcessInboxCommand>
    {
        private readonly ClusterContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<ProcessInboxCommandHandler> _logger;

        public ProcessInboxCommandHandler(
            ClusterContext context,
            IMediator mediator,
            ILogger<ProcessInboxCommandHandler> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(ProcessInboxCommand request, CancellationToken cancellationToken)
        {
            var messages = await _context.InboxMessages
                .Where(m => m.ProcessedDate == null)
                .OrderBy(m => m.ProcessedDate)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    await _mediator.Publish(DeserializeMessage(message), cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
                
                message.ProcessedDate = DateTime.UtcNow;
            }
            
            return Unit.Value;
        }

        private IEvent DeserializeMessage(InboxMessage message)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == message.Type);

            return (IEvent) JsonSerializer.Deserialize(message.Data, type);
        }
    }
}