using MediatR;
using System.Collections.Generic;

namespace Aimrank.Cluster.Core.Queries.GetSteamTokens
{
    public class GetSteamTokensQuery : IRequest<IEnumerable<SteamTokenDto>>
    {
    }
}