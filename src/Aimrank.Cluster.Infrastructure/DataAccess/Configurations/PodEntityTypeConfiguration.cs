using Aimrank.Cluster.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Aimrank.Cluster.Infrastructure.DataAccess.Configurations
{
    internal class PodEntityTypeConfiguration : IEntityTypeConfiguration<Pod>
    {
        public void Configure(EntityTypeBuilder<Pod> builder)
        {
            builder.HasKey(p => p.IpAddress);
            builder.Property(p => p.IpAddress).HasMaxLength(32);
        }
    }
}