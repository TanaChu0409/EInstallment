using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.CreditCards.Commands.UpdateCreditCard;
public sealed record UpdateCreditCardCommand(
    Guid CreditCardId,
    string CreditCardName,
    int PaymentDay) : ICommand;