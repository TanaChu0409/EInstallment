using EInstallment.Domain.Installments;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;

namespace EInstallment.Domain.CreditCards;

public sealed class CreditCard : Entity
{
    private readonly List<Installment> _installments = new();
    private readonly List<Payment> _payments = new();

    private CreditCard(
        Guid id,
        string bankName,
        DateTime paymentDateOnUtc)
        : base(id)
    {
        BankName = bankName;
        PaymentDateOnUtc = paymentDateOnUtc;
    }

    public string BankName { get; set; }

    public DateTime PaymentDateOnUtc { get; set; }

    public IReadOnlyCollection<Installment> Installments => _installments;

    public IReadOnlyCollection<Payment> Payments => _payments;

    public static CreditCard Create(
        string bankName,
        DateTime paymentDateOnUtc)
    {
        var creditCard = new CreditCard(
            Guid.NewGuid(),
            bankName,
            paymentDateOnUtc);

        return creditCard;
    }
}