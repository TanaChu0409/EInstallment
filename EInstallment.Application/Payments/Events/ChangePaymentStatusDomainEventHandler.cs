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

    public async Task Handle(ChangePaymentStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository
            .GetByIdAsync(notification.PaymentId, cancellationToken)
            .ConfigureAwait(false);

        if (payment is null)
        {
            return;
        }

        if (notification.Error is not null)
        {
            payment.ChangeStatus(PaymentStatus.Failed);
            payment.SetErrorMessage(notification.Error.Message);
        }
        else
        {
            payment.ChangeStatus(PaymentStatus.Success);
            payment.ClearErrorMessage();
        }

        _paymentRepository.Update(payment);

        await _unitOfWork
            .SaveEntitiesAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}