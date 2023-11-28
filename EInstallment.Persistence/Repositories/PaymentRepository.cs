using EInstallment.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly EInstallmentContext _context;

    public PaymentRepository(EInstallmentContext context)
    {
        _context = context;
    }

    public void Create(Payment payment)
    {
        _context.Set<Payment>().Add(payment);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Payment>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}