using EInstallment.Domain.CreditCards;
using EInstallment.Domain.DomainEvents.Payments;
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
        decimal lastAmount,
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
        LastAmount = lastAmount;
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

    public decimal LastAmount { get; private set; }

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

    public Result ReCalculation(decimal paymentAmount, Guid paymentId)
    {
        if (NotPayNumberOfInstallment < 1)
        {
            RaiseDomainEvent(new ChangePaymentStatusDomainEvent(
                paymentId,
                DomainErrors.Installment.CanNotPaymentAtNotPayNumberOfInstallIsZero));
            return Result.Failure(DomainErrors.Installment.CanNotPaymentAtNotPayNumberOfInstallIsZero);
        }

        if (AlreadyPayNumberOfInstallment >= TotalNumberOfInstallment)
        {
            RaiseDomainEvent(new ChangePaymentStatusDomainEvent(
                paymentId,
                DomainErrors.Installment.CanNotPaymentAtAlreadyPayNumberOfInstallmentIsGreaterOrEqualThanTotalNumberOfInstallment));
            return Result.Failure(DomainErrors.Installment.CanNotPaymentAtAlreadyPayNumberOfInstallmentIsGreaterOrEqualThanTotalNumberOfInstallment);
        }

        if (Status == InstallmentStatus.Upcoming)
        {
            Status = InstallmentStatus.Open;
        }

        LastAmount -= paymentAmount;

        if (LastAmount < 0m)
        {
            LastAmount = 0m;
        }

        --NotPayNumberOfInstallment;

        if (NotPayNumberOfInstallment < 1)
        {
            Status = InstallmentStatus.Finish;
        }

        var paymentInfo = _payments.Find(x => x.Id == paymentId);
        if (paymentInfo is not null)
        {
            RaiseDomainEvent(new ChangePaymentStatusDomainEvent(paymentId, null));
        }

        return Result.Success();
    }

    public Result CloseInstallment()
    {
        if (Status is not InstallmentStatus.Finish)
        {
            return Result.Failure(DomainErrors.Installment.InstallmentStillOpening);
        }

        Status = InstallmentStatus.Close;
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