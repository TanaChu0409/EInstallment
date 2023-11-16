namespace EInstallment.Api.Controllers.Installments;

public sealed record CreateInstallmentRequest(
    string ItemName,
    int TotalNumberOfInstallment,
    decimal TotalAmount,
    decimal AmountOfEachInstallment,
    Guid MemberId,
    Guid CreditCardId);