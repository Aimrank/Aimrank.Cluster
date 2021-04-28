using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aimrank.Cluster.Infrastructure.Processing.Inbox
{
    internal class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Type).HasMaxLength(256);
            builder.Property(m => m.Data);
            builder.Property(m => m.OccurredOn);
        }
    }
}