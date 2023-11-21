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

        paymentBuilder.Ignore(x => x.Installment);
        paymentBuilder.Ignore(x => x.Creator);

        paymentBuilder.HasOne(x => x.Installment)
                      .WithMany(x => x.Payments)
                      .HasForeignKey(x => x.InstallmentId);

        paymentBuilder.HasOne(x => x.Creator)
                      .WithMany(x => x.Payments)
                      .HasForeignKey(x => x.CreatorId);
    }
}