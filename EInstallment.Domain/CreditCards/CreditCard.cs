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
        BankName bankName,
        DateTime paymentDateOnUtc)
        : base(id)
    {
        BankName = bankName;
        PaymentDateOnUtc = paymentDateOnUtc;
    }

    public BankName BankName { get; set; }

    public DateTime PaymentDateOnUtc { get; set; }

    public IReadOnlyCollection<Installment> Installments => _installments;

    public IReadOnlyCollection<Payment> Payments => _payments;

    public static Result<CreditCard> Create(
        BankName bankName,
        DateTime paymentDateOnUtc)
    {
        var creditCard = new CreditCard(
            Guid.NewGuid(),
            bankName,
            paymentDateOnUtc);

        return creditCard;
    }
}