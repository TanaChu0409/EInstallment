using EInstallment.Domain.SeedWork;

namespace EInstallment.Persistence;

public sealed class EInstallmentContext : IUnitOfWork
{
    public void Dispose()
    {
    }

    public Task SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}