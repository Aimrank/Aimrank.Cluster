using Aimrank.Cluster.Core.Queries.GetSteamTokens;
using Aimrank.Cluster.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Cluster.Infrastructure.Queries
{
    internal class GetSteamTokensQueryHandler : IRequestHandler<GetSteamTokensQuery, IEnumerable<SteamTokenDto>>
    {
        private readonly ClusterContext _context;

        public GetSteamTokensQueryHandler(ClusterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SteamTokenDto>> Handle(GetSteamTokensQuery request, CancellationToken cancellationToken)
            => await _context.SteamTokens
                .Include(t => t.Server)
                .Select(t => new SteamTokenDto
                {
                    Token = t.Token,
                    IsUsed = t.Server != null
                })
                .OrderBy(t => t.IsUsed)
                .ToListAsync(cancellationToken);
    }
}