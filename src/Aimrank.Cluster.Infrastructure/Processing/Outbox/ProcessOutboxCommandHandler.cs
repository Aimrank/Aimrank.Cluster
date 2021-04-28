using Aimrank.Cluster.Core.Events;
using Aimrank.Cluster.Infrastructure.DataAccess;
using Aimrank.Cluster.Infrastructure.EventBus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Aimrank.Cluster.Infrastructure.Processing.Outbox
{
    internal class ProcessOutboxCommandHandler : IRequestHandler<ProcessOutboxCommand>
    {
        private readonly ClusterContext _context;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ProcessOutboxCommandHandler> _logger;

        public ProcessOutboxCommandHandler(
            ClusterContext context,
            IEventBus eventBus,
            ILogger<ProcessOutboxCommandHandler> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
        {
            var messages = await _context.OutboxMessages
                .Where(m => m.ProcessedDate == null)
                .OrderBy(m => m.ProcessedDate)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    _eventBus.Publish(DeserializeMessage(message));
                    
                    message.ProcessedDate = DateTime.UtcNow;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            }
            
            return Unit.Value;
        }

        private IEvent DeserializeMessage(OutboxMessage message)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == message.Type);

            return (IEvent) JsonSerializer.Deserialize(message.Data, type);
        }
    }
}