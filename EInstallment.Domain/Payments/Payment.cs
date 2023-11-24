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
        Installment installment)
        : base(id)
    {
        Amount = amount;
        CreateOnUtc = DateTime.UtcNow;
        Creator = creator;
        CreatorId = creator.Id;
        Installment = installment;
        InstallmentId = installment.Id;
    }

    public decimal Amount { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public Member Creator { get; private set; }

    public Guid CreatorId { get; private set; }

    public Installment Installment { get; private set; }

    public Guid InstallmentId { get; private set; }

    public static Result<Payment> Create(
        decimal amount,
        Member creator,
        Installment installment)
    {
        var payment = new Payment(
            Guid.NewGuid(),
            amount,
            creator,
            installment);

        // Raise domain event for re-calculated installment

        return payment;
    }
}