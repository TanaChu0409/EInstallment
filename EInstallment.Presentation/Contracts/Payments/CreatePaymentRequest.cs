namespace EInstallment.Api.Contracts.Payments;

public sealed record CreatePaymentRequest(
    decimal Amount,
    Guid CreatorId,
    Guid InstallmentId);