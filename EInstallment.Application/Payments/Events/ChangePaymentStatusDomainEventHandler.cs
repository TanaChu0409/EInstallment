using EInstallment.Domain.DomainEvents.Payments;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using MediatR;

namespace EInstallment.Application.Payments.Events;

internal sealed class ChangePaymentStatusDomainEventHandler
    : INotificationHandler<ChangePaymentStatusDomainEvent>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePaymentStatusDomainEventHandler(
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Handle(ChangePaymentStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}