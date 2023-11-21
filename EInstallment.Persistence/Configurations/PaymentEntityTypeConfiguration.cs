using EInstallment.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:避免未具現化的內部類別", Justification = "<暫止>")]
internal sealed class PaymentEntityTypeConfiguration
    : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> paymentBuilder)
    {
        paymentBuilder.ToTable(nameof(Payment));
        paymentBuilder.HasKey(x => x.Id);

        paymentBuilder.Property(x => x.Amount)
                      .HasPrecision(10, 1);
    }
}