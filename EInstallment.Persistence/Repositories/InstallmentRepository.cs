using EInstallment.Domain.Installments;
using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence.Repositories;

public sealed class InstallmentRepository : IInstallmentRepository
{
    private readonly EInstallmentContext _context;

    public InstallmentRepository(EInstallmentContext context)
    {
        _context = context;
    }

    public void Create(Installment installment)
    {
        _context.Set<Installment>().Add(installment);
    }

    public void Delete(Installment installment)
    {
        _context.Set<Installment>().Remove(installment);
    }

    public async Task<IReadOnlyCollection<Installment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Installment>()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Installment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Installment>()
             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
             .ConfigureAwait(false);
    }

    public void Update(Installment installment)
    {
        _context.Set<Installment>().Update(installment);
    }
}