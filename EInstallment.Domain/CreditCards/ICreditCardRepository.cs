using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.CreditCards;

public interface ICreditCardRepository
{
    Task<CreditCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CreditCard>> GetAllAsync(CancellationToken cancellationToken);

    void Create(CreditCard creditCard);

    void Update(CreditCard creditCard);

    void Delete(CreditCard creditCard);

    Task<bool> IsCreditCardNameUniqueAsync(CreditCardName creditCardName, CancellationToken cancellationToken);

    Task<bool> IsCreditCardNameUniqueWithoutItSelfAsync(Guid id, CreditCardName creditCardName, CancellationToken cancellationToken);
}