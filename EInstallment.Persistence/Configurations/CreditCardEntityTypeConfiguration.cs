using EInstallment.Domain.CreditCards;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

internal class CreditCardEntityTypeConfiguration
    : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> creditCardBuilder)
    {
        creditCardBuilder.ToTable(nameof(CreditCard));
        creditCardBuilder.HasKey(x => x.Id);

        creditCardBuilder.OwnsOne(x => x.Name)
                         .Property(x => x.Value)
                         .HasColumnName(nameof(CreditCard.Name))
                         .IsUnicode(true)
                         .HasMaxLength(CreditCardName.MaxLength);
        //creditCardBuilder.HasMany(x => x.Installments)
        //                 .WithOne(x => x.CreditCard)
        //                 .HasForeignKey(x => x.CreditCard.Id);
    }
}