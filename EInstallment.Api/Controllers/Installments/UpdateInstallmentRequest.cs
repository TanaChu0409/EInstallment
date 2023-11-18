namespace EInstallment.Api.Controllers.Installments;

public sealed record UpdateInstallmentRequest(
    Guid InstallmentId,
    string ItemName,
    int TotalNumberOfInstallment,
    decimal TotalAmount,
    decimal AmountOfEachInstallment);