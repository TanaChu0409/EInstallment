using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence;

public sealed class EInstallmentContext : DbContext, IUnitOfWork
{
    public EInstallmentContext(
        DbContextOptions<EInstallmentContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members { get; set; }

    public DbSet<CreditCard> CreditCards { get; set; }

    public DbSet<Installment> Installments { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CreditCardEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InstallmentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
    }
}