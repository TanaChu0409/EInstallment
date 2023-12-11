namespace EInstallment.Api.Contracts.CreditCards;

public sealed record CreateCreditCardRequest(
    string CreditCardName,
    int PaymentDay);