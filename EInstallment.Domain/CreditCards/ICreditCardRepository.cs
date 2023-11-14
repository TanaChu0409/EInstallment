using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.CreditCards;

public interface ICreditCardRepository
{
    Task<CreditCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CreditCard>> GetAllAsync(CancellationToken cancellationToken);

    void Create(CreditCard creditCard, CancellationToken cancellationToken);

    void Update(CreditCard creditCard, CancellationToken cancellationToken);

    void Delete(CreditCard creditCard, CancellationToken cancellationToken);

    Task<bool> IsCreditCardNameUniqueAsync(CreditCardName creditCardName, CancellationToken cancellationToken);

    Task<bool> IsCreditCardNameUniqueWithoutItSelfAsync(Guid Id, CreditCardName creditCardName, CancellationToken cancellationToken);
}