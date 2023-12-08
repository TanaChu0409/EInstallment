using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.DomainEvents.Payments;
public record ChangePaymentStatusDomainEvent(
    Guid PaymentId,
    Error? Error) : IDomainEvent;