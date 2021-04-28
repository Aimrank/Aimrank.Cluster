using Aimrank.Cluster.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Core.Repositories
{
    public interface ISteamTokenRepository
    {
        Task<IEnumerable<SteamToken>> BrowseUnusedAsync(int limit);
        Task<SteamToken> GetAsync(string token);
        Task<SteamToken> GetOptionalAsync(string token);
        void Add(SteamToken steamToken);
        void Delete(SteamToken steamToken);
    }
}