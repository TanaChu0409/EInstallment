namespace EInstallment.Domain.Payments;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    void Create(Payment payment);

    void Update(Payment payment);
}