using Aimrank.Cluster.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Configurations
{
    internal class SteamTokenEntityTypeConfiguration : IEntityTypeConfiguration<SteamToken>
    {
        public void Configure(EntityTypeBuilder<SteamToken> builder)
        {
            builder.HasKey(t => t.Token);
            builder.Property(t => t.Token).HasMaxLength(32);
        }
    }
}