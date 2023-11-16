using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.Installments.Commands.CreateInstallment;

internal sealed class CreateInstallmentCommandHandler : ICommandHandler<CreateInstallmentCommand, Guid>
{
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInstallmentCommandHandler(
        IInstallmentRepository installmentRepository,
        IMemberRepository memberRepository,
        ICreditCardRepository creditCardRepository,
        IUnitOfWork unitOfWork)
    {
        _installmentRepository = installmentRepository;
        _memberRepository = memberRepository;
        _creditCardRepository = creditCardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateInstallmentCommand request, CancellationToken cancellationToken)
    {
        var itemName = ItemName.Create(request.ItemName);
        if (itemName.IsFailure)
        {
            return Result.Failure<Guid>(itemName.Error);
        }

        var creator = await _memberRepository
                                .GetByIdAsync(request.MemberId, cancellationToken)
                                .ConfigureAwait(false);

        if (creator is null)
        {
            return Result.Failure<Guid>(new Error(
                "EInstallment.CreateInstallmentHandler",
                $"The member id {request.MemberId} is not exist"));
        }

        var creditCard = await _creditCardRepository
                                .GetByIdAsync(request.CreditCardId, cancellationToken)
                                .ConfigureAwait(false);

        if (creditCard is null)
        {
            return Result.Failure<Guid>(new Error(
                "EInstallment.CreateInstallmentHandler",
                $"The credit card id {request.CreditCardId} is not exist"));
        }

        var installment = Installment.Create(
                            itemName.Value,
                            request.TotalNumberOfInstallment,
                            request.TotalAmount,
                            request.AmountOfEachInstallment,
                            creator,
                            creditCard);

        if (installment.IsFailure)
        {
            return Result.Failure<Guid>(installment.Error);
        }

        _installmentRepository.Create(installment.Value);
        await _unitOfWork
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

        return Result.Success(installment.Value.Id);
    }
}