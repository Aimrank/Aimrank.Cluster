using Aimrank.Cluster.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Infrastructure.Processing.RemoveProcessedMessages
{
    internal class RemoveProcessedMessagesCommandHandler : IRequestHandler<RemoveProcessedMessagesCommand>
    {
        private readonly ClusterContext _context;

        public RemoveProcessedMessagesCommandHandler(ClusterContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveProcessedMessagesCommand request, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM cluster.inbox_messages
                WHERE EXTRACT(EPOCH FROM NOW() - processed_date) / 3600 >= 2;
                DELETE FROM cluster.outbox_messages
                WHERE EXTRACT(EPOCH FROM NOW() - processed_date) / 3600 >= 2;";

            await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            
            return Unit.Value;
        }
    }
}