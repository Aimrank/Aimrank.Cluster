using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Aimrank.Cluster.Infrastructure.Processing.Outbox
{
    internal class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Type).HasMaxLength(256);
            builder.Property(m => m.Data);
            builder.Property(m => m.OccurredOn);
        }
    }
}