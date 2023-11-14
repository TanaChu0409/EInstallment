using EInstallment.Application.CreditCards.Commands.CreateCreditCard;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.CreditCards;

public sealed class CreateCreditCardCommandValidator :
    AbstractValidator<CreateCreditCardCommand>
{
    public CreateCreditCardCommandValidator()
    {
        RuleFor(rule => rule.CreditCardName)
            .NotEmpty()
            .MaximumLength(CreditCardName.MaxLength);

        RuleFor(rule => rule.PaymentDay)
            .GreaterThan(CreditCardName.GreaterThan)
            .LessThan(CreditCardName.LessThan);
    }
}