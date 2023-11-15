namespace EInstallment.Domain.Installments;

public interface IInstallmentRepository
{
    Task<Installment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Installment>> GetAllAsync(CancellationToken cancellationToken);

    void Create(Installment installment);

    void Update(Installment installment);

    void Delete(Installment installment);
}