using EInstallment.Application.Installments.Commands.CreateInstallment;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.Installments;

public sealed class CreateInstallmentCommandValidator :
    AbstractValidator<CreateInstallmentCommand>
{
    public CreateInstallmentCommandValidator()
    {
        RuleFor(rule => rule.ItemName)
            .NotEmpty()
            .MaximumLength(ItemName.MaxLength);

        RuleFor(rule => rule.MemberId)
            .NotEmpty();

        RuleFor(rule => rule.CreditCardId)
            .NotEmpty();
    }
}