using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence;

public sealed class EInstallmentContext : DbContext
{
    public EInstallmentContext(DbContextOptions<EInstallmentContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}