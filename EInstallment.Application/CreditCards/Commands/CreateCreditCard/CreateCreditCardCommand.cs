using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.CreditCards.Commands.CreateCreditCard;
public sealed record CreateCreditCardCommand(
    string CreditCardName,
    int PaymentDay) : ICommand<Guid>;