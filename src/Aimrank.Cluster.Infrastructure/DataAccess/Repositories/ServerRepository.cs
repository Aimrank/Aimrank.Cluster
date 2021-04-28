using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Exceptions;
using Aimrank.Cluster.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Repositories
{
    internal sealed class ServerRepository : IServerRepository
    {
        private readonly ClusterContext _context;

        public ServerRepository(ClusterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Server>> BrowseByIpAddressesAsync(IEnumerable<string> ipAddresses)
            => await _context.Pods
                .Include(p => p.Servers)
                .Where(p => ipAddresses.Contains(p.IpAddress))
                .SelectMany(p => p.Servers)
                .ToListAsync();

        public async Task<Server> GetByIdAsync(Guid id)
        {
            var server = await _context.Servers
                .Include(s => s.Pod)
                .Include(s => s.SteamToken)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            if (server is null)
            {
                throw new NotFoundException($"Server with id '{id}' does not exist.");
            }

            return server;
        }

        public void Add(Server server) => _context.Servers.Add(server);

        public void Delete(Server server) => _context.Servers.Remove(server);

        public void DeleteRange(IEnumerable<Server> servers) => _context.Servers.RemoveRange(servers);
    }
}