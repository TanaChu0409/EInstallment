namespace EInstallment.Api.Controllers.CreditCards;

public sealed record CreateCreditCardRequest(
    string CreditCardName,
    int PaymentDay);