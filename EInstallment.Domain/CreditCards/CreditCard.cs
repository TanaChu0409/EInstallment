using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.CreditCards;

public sealed class CreditCard : Entity
{
    private readonly List<Installment> _installments = new();
    private readonly List<Payment> _payments = new();

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

    public IReadOnlyCollection<Payment> Payments => _payments;

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
        int paymentDay)
    {
        Name = creditCardName;
        PaymentDay = paymentDay;
        return Result.Success();
    }
}