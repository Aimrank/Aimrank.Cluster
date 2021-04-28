using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Infrastructure.Processing.Inbox;
using Aimrank.Cluster.Infrastructure.Processing.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Aimrank.Cluster.Infrastructure.DataAccess
{
    internal class ClusterContext : DbContext
    {
        public DbSet<Pod> Pods { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<SteamToken> SteamTokens { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public ClusterContext(DbContextOptions<ClusterContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cluster");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}