using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.Installments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.Installments.Commands.UpdateInstallment;

internal sealed class UpdateInstallmentCommandHandler : ICommandHandler<UpdateInstallmentCommand>
{
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInstallmentCommandHandler(
        IInstallmentRepository installmentRepository,
        IUnitOfWork unitOfWork)
    {
        _installmentRepository = installmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstallmentCommand request, CancellationToken cancellationToken)
    {
        var installment = await _installmentRepository
                                    .GetByIdAsync(request.InstallmentId, cancellationToken)
                                    .ConfigureAwait(false);

        if (installment is null)
        {
            return Result.Failure(new Error(
                "EInstallment.UpdateInstallmentHandler",
                "Can't find this installment"));
        }

        var itemName = ItemName.Create(request.ItemName);

        if (itemName.IsFailure)
        {
            return Result.Failure(itemName.Error);
        }

        var result = installment.Update(
            itemName.Value,
            request.TotalNumberOfInstallment,
            request.TotalAmount,
            request.AmountOfEachInstallment);

        if (result.IsFailure)
        {
            return result;
        }

        _installmentRepository.Update(installment);
        await _unitOfWork
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

        return result;
    }
}