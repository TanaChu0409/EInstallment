namespace EInstallment.Api.Controllers.CreditCards;

public sealed record UpdateCreditCardRequest(
    Guid CreditCardId,
    string CreditCardName,
    int PaymentDay);