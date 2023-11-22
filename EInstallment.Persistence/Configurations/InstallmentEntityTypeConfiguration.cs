using EInstallment.Domain.Installments;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:避免未具現化的內部類別", Justification = "<暫止>")]
internal sealed class InstallmentEntityTypeConfiguration
    : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> installmentBuilder)
    {
        installmentBuilder.ToTable(nameof(Installment));
        installmentBuilder.HasKey(x => x.Id);

        installmentBuilder
            .Property(x => x.ItemName)
            .HasConversion(x => x.Value, v => ItemName.Create(v).Value)
            .HasMaxLength(ItemName.MaxLength);

        installmentBuilder.Ignore(x => x.Creator);
        installmentBuilder.Ignore(x => x.CreditCard);

        installmentBuilder.Property(x => x.TotalAmount)
                          .HasPrecision(10, 1);
        installmentBuilder.Property(x => x.AmountOfEachInstallment)
                          .HasPrecision(10, 2);

        installmentBuilder.HasOne(x => x.Creator)
                          .WithMany(x => x.Installments)
                          .HasForeignKey(x => x.CreatorId);
        installmentBuilder.HasOne(x => x.CreditCard)
                          .WithMany(x => x.Installments)
                          .HasForeignKey(x => x.CreditCardId);
    }
}