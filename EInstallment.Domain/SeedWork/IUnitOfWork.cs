namespace EInstallment.Domain.SeedWork;

public interface IUnitOfWork
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken);
}