using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Members;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.Installments;

public sealed class Installment : Entity
{
    private readonly List<Payment> _payments = new();

    private Installment(
        Guid id,
        ItemName itemName,
        int totalNumberOfInstallment,
        int alreadyPayNumberOfInstallment,
        int notPayNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment,
        Member creator,
        CreditCard creditCard)
        : base(id)
    {
        ItemName = itemName;
        TotalNumberOfInstallment = totalNumberOfInstallment;
        AlreadyPayNumberOfInstallment = alreadyPayNumberOfInstallment;
        NotPayNumberOfInstallment = notPayNumberOfInstallment;
        TotalAmount = totalAmount;
        AmountOfEachInstallment = amountOfEachInstallment;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreditCard = creditCard;
    }

    public ItemName ItemName { get; private set; }

    public int TotalNumberOfInstallment { get; private set; }

    public int AlreadyPayNumberOfInstallment { get; private set; }

    public int NotPayNumberOfInstallment { get; private set; }

    public decimal TotalAmount { get; private set; }

    public decimal AmountOfEachInstallment { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public IReadOnlyCollection<Payment> Payments => _payments;

    public CreditCard CreditCard { get; private set; }

    public static Result<Installment> Create(
        ItemName itemName,
        int totalNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment,
        Member creator,
        CreditCard creditCard)
    {
        if (totalNumberOfInstallment <= 0)
        {
            return Result.Failure<Installment>(DomainErrors.Installment.TotalNumberOfInstallmentLessThanOne);
        }

        if (totalAmount <= 0.0m)
        {
            return Result.Failure<Installment>(DomainErrors.Installment.TotalAmountLessThanOne);
        }

        if (amountOfEachInstallment < 0.0m)
        {
            return Result.Failure<Installment>(DomainErrors.Installment.AmountOfEachInstallmentLessThanOne);
        }

        var installment = new Installment(
            Guid.NewGuid(),
            itemName,
            totalNumberOfInstallment,
            0,
            totalNumberOfInstallment,
            totalAmount,
            amountOfEachInstallment,
            creator,
            creditCard);

        return Result.Success(installment);
    }
}