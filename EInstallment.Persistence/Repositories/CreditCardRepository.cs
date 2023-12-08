using EInstallment.Domain.CreditCards;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence.Repositories;

public sealed class CreditCardRepository : ICreditCardRepository
{
    private readonly ApplicationDbContext _context;

    public CreditCardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(CreditCard creditCard)
    {
        _context.Set<CreditCard>().Add(creditCard);
    }

    public void Update(CreditCard creditCard)
    {
        _context.Set<CreditCard>().Update(creditCard);
    }

    public void Delete(CreditCard creditCard)
    {
        _context.Set<CreditCard>().Remove(creditCard);
    }

    public async Task<IReadOnlyCollection<CreditCard>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<CreditCard>()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<CreditCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<CreditCard>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> IsCreditCardNameUniqueAsync(CreditCardName creditCardName, CancellationToken cancellationToken)
    {
        return await _context.Set<CreditCard>()
            .AnyAsync(x => x.Name == creditCardName, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> IsCreditCardNameUniqueWithoutItSelfAsync(Guid id, CreditCardName creditCardName, CancellationToken cancellationToken)
    {
        return await _context.Set<CreditCard>()
             .AnyAsync(x => x.Id != id &&
                            x.Name == creditCardName,
                       cancellationToken)
             .ConfigureAwait(false);
    }
}