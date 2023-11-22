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
    protected Installment()
    {
    }

    private readonly List<Payment> _payments = new();

    private Installment(
        Guid id,
        ItemName itemName,
        int totalNumberOfInstallment,
        int alreadyPayNumberOfInstallment,
        int notPayNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment,
        InstallmentStatus status,
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
        Status = status;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreatorId = creator.Id;
        CreditCard = creditCard;
        CreditCardId = creditCard.Id;
    }

    public ItemName ItemName { get; private set; }

    public int TotalNumberOfInstallment { get; private set; }

    public int AlreadyPayNumberOfInstallment { get; private set; }

    public int NotPayNumberOfInstallment { get; private set; }

    public decimal TotalAmount { get; private set; }

    public decimal AmountOfEachInstallment { get; private set; }

    public InstallmentStatus Status { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public Guid CreatorId { get; private set; }

    public IReadOnlyCollection<Payment> Payments => _payments;

    public CreditCard CreditCard { get; private set; }

    public Guid CreditCardId { get; private set; }

    public static Result<Installment> Create(
        ItemName itemName,
        int totalNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment,
        Member creator,
        CreditCard creditCard)
    {
        var verifyResult = VerifyInstallmentConditionIsSatisfaction(
            totalNumberOfInstallment,
            totalAmount,
            amountOfEachInstallment);

        if (verifyResult.IsFailure)
        {
            return Result.Failure<Installment>(verifyResult.Error);
        }

        var installment = new Installment(
            Guid.NewGuid(),
            itemName,
            totalNumberOfInstallment,
            0,
            totalNumberOfInstallment,
            totalAmount,
            amountOfEachInstallment,
            InstallmentStatus.Upcoming,
            creator,
            creditCard);

        return Result.Success(installment);
    }

    public Result Update(
        ItemName itemName,
        int totalNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment)
    {
        if (Status is not InstallmentStatus.Upcoming)
        {
            return Result.Failure(DomainErrors.Installment.StatusIsNotUpcoming);
        }

        var verifyResult = VerifyInstallmentConditionIsSatisfaction(
            totalNumberOfInstallment,
            totalAmount,
            amountOfEachInstallment);

        if (verifyResult.IsFailure)
        {
            return verifyResult;
        }

        ItemName = itemName;
        TotalNumberOfInstallment = totalNumberOfInstallment;
        TotalAmount = totalAmount;
        AmountOfEachInstallment = amountOfEachInstallment;
        ModifiedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    private static Result VerifyInstallmentConditionIsSatisfaction(
        int totalNumberOfInstallment,
        decimal totalAmount,
        decimal amountOfEachInstallment)
    {
        if (totalNumberOfInstallment <= 0)
        {
            return Result.Failure(DomainErrors.Installment.TotalNumberOfInstallmentLessThanOne);
        }

        if (totalAmount <= 0.0m)
        {
            return Result.Failure(DomainErrors.Installment.TotalAmountLessThanOne);
        }

        if (amountOfEachInstallment <= 0.0m)
        {
            return Result.Failure(DomainErrors.Installment.AmountOfEachInstallmentLessThanOne);
        }

        return Result.Success();
    }
}