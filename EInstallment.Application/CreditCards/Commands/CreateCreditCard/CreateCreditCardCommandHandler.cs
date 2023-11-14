using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.CreditCards.Commands.CreateCreditCard;

internal sealed class CreateCreditCardCommandHandler : ICommandHandler<CreateCreditCardCommand, Guid>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCreditCardCommandHandler(
        ICreditCardRepository creditCardRepository,
        IUnitOfWork unitOfWork)
    {
        _creditCardRepository = creditCardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var creditCardName = CreditCardName.Create(request.CreditCardName);
        if (creditCardName.IsFailure)
        {
            return Result.Failure<Guid>(creditCardName.Error);
        }

        var isCreditCardNameUnique = await _creditCardRepository
                    .IsCreditCardNameUniqueAsync(creditCardName.Value, cancellationToken)
                    .ConfigureAwait(false);

        var creditCard = CreditCard.Create(
            creditCardName.Value,
            request.PaymentDay,
            isCreditCardNameUnique);

        if (creditCard.IsFailure)
        {
            return Result.Failure<Guid>(creditCard.Error);
        }

        _creditCardRepository.Create(creditCard.Value, cancellationToken);
        await _unitOfWork
            .SaveEntitiesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(creditCard.Value.Id);
    }
}