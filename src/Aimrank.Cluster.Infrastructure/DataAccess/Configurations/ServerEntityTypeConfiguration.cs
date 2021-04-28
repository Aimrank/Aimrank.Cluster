using Aimrank.Cluster.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Configurations
{
    internal class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .HasOne(s => s.SteamToken)
                .WithOne(t => t.Server)
                .HasForeignKey<Server>();

            builder.Navigation(s => s.Pod).IsRequired();
            builder.Navigation(s => s.SteamToken).IsRequired();
        }
    }
}