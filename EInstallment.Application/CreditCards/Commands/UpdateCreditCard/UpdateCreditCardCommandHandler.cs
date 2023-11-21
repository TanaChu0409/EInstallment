using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.CreditCards.Commands.UpdateCreditCard;

internal sealed class UpdateCreditCardCommandHandler : ICommandHandler<UpdateCreditCardCommand>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCreditCardCommandHandler(
        ICreditCardRepository creditCardRepository,
        IUnitOfWork unitOfWork)
    {
        _creditCardRepository = creditCardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var creditCardName = CreditCardName.Create(request.CreditCardName);
        if (creditCardName.IsFailure)
        {
            return Result.Failure(creditCardName.Error);
        }

        var creditCard = await _creditCardRepository
            .GetByIdAsync(request.CreditCardId, cancellationToken)
            .ConfigureAwait(false);

        if (creditCard is null)
        {
            return Result.Failure(new Error(
                "EInstallment.UpdateCreditCard",
                "Credit card is not exist"));
        }

        var isCreditCardNameUniqueWithoutItSelf = await _creditCardRepository
                    .IsCreditCardNameUniqueWithoutItSelfAsync(creditCard.Id, creditCardName.Value, cancellationToken)
                    .ConfigureAwait(false);

        var result = creditCard.Update(
            creditCardName.Value,
            request.PaymentDay,
            isCreditCardNameUniqueWithoutItSelf);

        if (result.IsFailure)
        {
            return result;
        }

        _creditCardRepository.Update(creditCard);
        await _unitOfWork
            .SaveEntitiesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success();
    }
}