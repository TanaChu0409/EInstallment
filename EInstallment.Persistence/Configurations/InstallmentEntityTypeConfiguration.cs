using EInstallment.Domain.Installments;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

internal class InstallmentEntityTypeConfiguration
    : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> installmentBuilder)
    {
        installmentBuilder.ToTable(nameof(Installment));
        installmentBuilder.HasKey(x => x.Id);

        installmentBuilder.OwnsOne(x => x.ItemName)
                          .Property(x => x.Value)
                          .HasColumnName(nameof(ItemName))
                          .IsUnicode(true)
                          .HasMaxLength(ItemName.MaxLength);

        installmentBuilder.Property(x => x.TotalAmount)
                          .HasPrecision(10, 1);
        installmentBuilder.Property(x => x.AmountOfEachInstallment)
                          .HasPrecision(10, 2);
    }
}