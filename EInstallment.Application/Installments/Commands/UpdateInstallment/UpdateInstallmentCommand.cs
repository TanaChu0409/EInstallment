using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.Installments.Commands.UpdateInstallment;
public sealed record UpdateInstallmentCommand(
    Guid InstallmentId,
    string ItemName,
    int TotalNumberOfInstallment,
    decimal TotalAmount,
    decimal AmountOfEachInstallment) : ICommand;