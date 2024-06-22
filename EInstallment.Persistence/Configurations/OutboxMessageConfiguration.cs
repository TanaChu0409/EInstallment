using EInstallment.Persistence.Constants;
using EInstallment.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> outboxMessageBuilder)
    {
        outboxMessageBuilder.ToTable(TableNames.OutboxMessage);
        outboxMessageBuilder.HasKey(outboxMsg => outboxMsg.Id);
    }
}