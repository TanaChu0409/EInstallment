using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;

namespace EInstallment.Domain.Payments;

public sealed class Payment : Entity
{
    private Payment(
        Guid id,
        decimal amount,
        Member creator,
        CreditCard creditCard,
        Installment installment)
        : base(id)
    {
        Amount = amount;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreditCard = creditCard;
        Installment = installment;
    }

    public decimal Amount { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public CreditCard CreditCard { get; private set; }

    public Installment Installment { get; private set; }

    public static Payment Create(
        decimal amount,
        Member creator,
        CreditCard creditCard,
        Installment installment)
    {
        var payment = new Payment(
            Guid.NewGuid(),
            amount,
            creator,
            creditCard,
            installment);

        // missing for change installment info

        return payment;
    }
}