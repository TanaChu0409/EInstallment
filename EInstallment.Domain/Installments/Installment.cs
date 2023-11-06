using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Members;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.Installments;

public sealed class Installment : Entity
{
    private readonly List<Payment> _payments = new();

    private Installment(
        Guid id,
        string itemName,
        int totalNumberOfInstallment,
        int alreadyPayNumberOfInstallment,
        int notPayNumberOfInstallment,
        decimal totalAmount,
        decimal amountForEachInstallment,
        Member creator,
        CreditCard creditCard)
        : base(id)
    {
        ItemName = itemName;
        TotalNumberOfInstallment = totalNumberOfInstallment;
        AlreadyPayNumberOfInstallment = alreadyPayNumberOfInstallment;
        AmountForEachInstallment = amountForEachInstallment;
        NotPayNumberOfInstallment = notPayNumberOfInstallment;
        TotalAmount = totalAmount;
        AmountForEachInstallment = amountForEachInstallment;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreditCard = creditCard;
    }

    public string ItemName { get; private set; }

    public int TotalNumberOfInstallment { get; private set; }

    public int AlreadyPayNumberOfInstallment { get; private set; }

    public int NotPayNumberOfInstallment { get; private set; }

    public decimal TotalAmount { get; private set; }

    public decimal AmountForEachInstallment { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public IReadOnlyCollection<Payment> Payments => _payments;

    public CreditCard CreditCard { get; private set; }

    public static Result<Installment> Create(
        string itemName,
        int totalNumberOfInstallment,
        int alreadyPayNumberOfInstallment,
        int notPayNumberOfInstallment,
        decimal totalAmount,
        decimal amountForEachInstallment,
        Member creator,
        CreditCard creditCard)
    {
        var installment = new Installment(
            Guid.NewGuid(),
            itemName,
            totalNumberOfInstallment,
            alreadyPayNumberOfInstallment,
            notPayNumberOfInstallment,
            totalAmount,
            amountForEachInstallment,
            creator,
            creditCard);

        return installment;
    }
}