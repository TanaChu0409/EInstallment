namespace EInstallment.Api.Contracts.CreditCards;

public sealed record UpdateCreditCardRequest(
    Guid CreditCardId,
    string CreditCardName,
    int PaymentDay);