using EInstallment.Domain.SeedWork;

namespace EInstallment.Persistence;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:避免未具現化的內部類別", Justification = "<暫止>")]
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task SaveEntitiesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}