using EInstallment.Domain.CreditCards;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:避免未具現化的內部類別", Justification = "<暫止>")]
internal sealed class CreditCardEntityTypeConfiguration
    : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> creditCardBuilder)
    {
        creditCardBuilder.ToTable(nameof(CreditCard));
        creditCardBuilder.HasKey(x => x.Id);

        creditCardBuilder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, v => CreditCardName.Create(v).Value)
            .HasMaxLength(CreditCardName.MaxLength);
    }
}