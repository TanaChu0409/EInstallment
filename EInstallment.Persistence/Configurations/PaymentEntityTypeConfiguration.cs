using EInstallment.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

internal class PaymentEntityTypeConfiguration
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