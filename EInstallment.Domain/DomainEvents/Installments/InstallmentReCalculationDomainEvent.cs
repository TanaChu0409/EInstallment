using EInstallment.Domain.SeedWork;

namespace EInstallment.Domain.DomainEvents.Installments;
public sealed record InstallmentReCalculationDomainEvent(
    decimal PaymentAmount,
    Guid InstallmentId,
    Guid PaymentId) : IDomainEvent;