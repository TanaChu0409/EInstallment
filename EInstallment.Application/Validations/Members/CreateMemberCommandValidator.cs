using EInstallment.Application.Members.Commands.CreateMember;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.Members;

public sealed class CreateMemberCommandValidator :
    AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(rule => rule.FirstName)
            .NotEmpty()
            .MaximumLength(FirstName.MaxLength);
        RuleFor(rule => rule.LastName)
            .NotEmpty()
            .MaximumLength(LastName.MaxLength);
        RuleFor(rule => rule.Email)
            .NotEmpty()
            .MaximumLength(Email.MaxLength);
    }
}