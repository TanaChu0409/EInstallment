using EInstallment.Application.CreditCards.Commands.UpdateCreditCard;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.CreditCards;

public sealed class UpdateCreditCardCommandValidator :
    AbstractValidator<UpdateCreditCardCommand>
{
    public UpdateCreditCardCommandValidator()
    {
        RuleFor(rule => rule.CreditCardId)
            .NotEmpty();
        RuleFor(rule => rule.CreditCardName)
            .NotEmpty()
            .MaximumLength(CreditCardName.MaxLength);
        RuleFor(rule => rule.PaymentDay)
            .GreaterThan(CreditCardName.GreaterThan)
            .LessThan(CreditCardName.LessThan);
    }
}