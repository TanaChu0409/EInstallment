using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.CreditCards;

public sealed class CreditCard : Entity
{
    private readonly List<Installment> _installments = new();

    private CreditCard(
        Guid id,
        CreditCardName name,
        int paymentDay)
        : base(id)
    {
        Name = name;
        PaymentDay = paymentDay;
    }

    public CreditCardName Name { get; set; }

    public int PaymentDay { get; set; }

    public IReadOnlyCollection<Installment> Installments => _installments;

    public static Result<CreditCard> Create(
        CreditCardName creditCardName,
        int paymentDay,
        bool isCreditCardNameUnique)
    {
        if (!isCreditCardNameUnique)
        {
            return Result.Failure<CreditCard>(DomainErrors.CreditCard.CreditCardNameIsNotUnique);
        }

        var creditCard = new CreditCard(
            Guid.NewGuid(),
            creditCardName,
            paymentDay);

        return creditCard;
    }

    public Result Update(
        CreditCardName creditCardName,
        int paymentDay,
        bool isCreditCardNameUniqueWithoutItSelf)
    {
        if (!isCreditCardNameUniqueWithoutItSelf)
        {
            return Result.Failure(DomainErrors.CreditCard.CreditCardNameIsNotUnique);
        }

        Name = creditCardName;
        PaymentDay = paymentDay;
        return Result.Success();
    }
}