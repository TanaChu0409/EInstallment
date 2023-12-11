namespace EInstallment.Api.Contracts.Installments;

public sealed record UpdateInstallmentRequest(
    Guid InstallmentId,
    string ItemName,
    int TotalNumberOfInstallment,
    decimal TotalAmount,
    decimal AmountOfEachInstallment);