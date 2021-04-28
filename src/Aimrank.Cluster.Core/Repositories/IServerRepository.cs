using Aimrank.Cluster.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Aimrank.Cluster.Core.Repositories
{
    public interface IServerRepository
    {
        Task<IEnumerable<Server>> BrowseByIpAddressesAsync(IEnumerable<string> ipAddresses);
        Task<Server> GetByIdAsync(Guid id);
        void Add(Server server);
        void Delete(Server server);
        void DeleteRange(IEnumerable<Server> servers);
    }
}