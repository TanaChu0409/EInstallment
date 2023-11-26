using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.Payments.Commands.CreatePayment;
public sealed record CreatePaymentCommand(
    decimal Amount,
    Guid CreatorId,
    Guid InstallmentId) : ICommand<Guid>;