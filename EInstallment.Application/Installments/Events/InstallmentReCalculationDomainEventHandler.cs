using EInstallment.Domain.DomainEvents.Installments;
using EInstallment.Domain.Installments;
using EInstallment.Domain.SeedWork;
using MediatR;

namespace EInstallment.Application.Installments.Events;

internal sealed class InstallmentReCalculationDomainEventHandler
    : INotificationHandler<InstallmentReCalculationDomainEvent>
{
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InstallmentReCalculationDomainEventHandler(
        IInstallmentRepository installmentRepository,
        IUnitOfWork unitOfWork)
    {
        _installmentRepository = installmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(InstallmentReCalculationDomainEvent notification, CancellationToken cancellationToken)
    {
        var installment = await _installmentRepository
            .GetByIdAsync(notification.InstallmentId, cancellationToken)
            .ConfigureAwait(false);

        if (installment is null)
        {
            throw new InvalidOperationException(
                $"Installment with id {notification.InstallmentId} not found.");
        }

        throw new NotImplementedException();
    }
}