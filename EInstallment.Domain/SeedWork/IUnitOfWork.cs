namespace EInstallment.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken);
}