using EInstallment.Application.Installments.Commands.UpdateInstallment;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.Installments;

public sealed class UpdateInstallmentCommandValidator
    : AbstractValidator<UpdateInstallmentCommand>
{
    public UpdateInstallmentCommandValidator()
    {
        RuleFor(rule => rule.InstallmentId)
            .NotEmpty();

        RuleFor(rule => rule.ItemName)
            .NotEmpty()
            .MaximumLength(ItemName.MaxLength);
    }
}