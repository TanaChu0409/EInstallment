using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.Installments.Commands.CreateInstallment;
public sealed record CreateInstallmentCommand(
    string ItemName,
    int TotalNumberOfInstallment,
    decimal TotalAmount,
    decimal AmountOfEachInstallment,
    Guid MemberId,
    Guid CreditCardId) : ICommand<Guid>;