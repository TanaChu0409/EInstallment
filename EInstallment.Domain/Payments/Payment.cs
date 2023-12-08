using EInstallment.Domain.DomainEvents.Installments;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.Payments;

public sealed class Payment : Entity
{
    protected Payment()
    {
    }

    private Payment(
        Guid id,
        decimal amount,
        Member creator,
        Installment installment,
        PaymentStatus status,
        string? errorMessage)
        : base(id)
    {
        Amount = amount;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreatorId = creator.Id;
        Installment = installment;
        InstallmentId = installment.Id;
        Status = status;
        ErrorMessage = errorMessage;
    }

    public decimal Amount { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public Guid CreatorId { get; private set; }

    public Installment Installment { get; private set; }

    public Guid InstallmentId { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string? ErrorMessage { get; private set; }

    public static Result<Payment> Create(
        decimal amount,
        Member creator,
        Installment installment)
    {
        var validationResult = ValidatePayment(amount, installment);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Payment>(validationResult.Error);
        }

        var payment = new Payment(
            Guid.NewGuid(),
            amount,
            creator,
            installment,
            PaymentStatus.Upcoming,
            null);

        return payment;
    }

    public void ChangeStatus(PaymentStatus status)
    {
        Status = status;
    }

    public void SetErrorMessage(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public void ClearErrorMessage()
    {
        ErrorMessage = null;
    }

    public void ReCalculation()
    {
        Status = PaymentStatus.Processing;
        RaiseDomainEvent(new InstallmentReCalculationDomainEvent(
            this.Amount,
            this.InstallmentId,
            this.Id));
    }

    public static Result ValidatePayment(decimal amount, Installment installment)
    {
        if (amount < 1m)
        {
            return Result.Failure(DomainErrors.Payment.AmountLessThanOne);
        }

        if (amount > installment.TotalAmount)
        {
            return Result.Failure(DomainErrors.Payment.AmountGreaterThanInstallmentAmount);
        }

        if (installment.Status == InstallmentStatus.Finish)
        {
            return Result.Failure(DomainErrors.Payment.InstallmentIsFinish);
        }

        if (installment.Status == InstallmentStatus.Close)
        {
            return Result.Failure(DomainErrors.Payment.InstallmentIsClose);
        }

        return Result.Success();
    }
}