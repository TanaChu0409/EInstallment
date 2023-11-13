using EInstallment.Application.Members.Commands.UpdateMember;
using EInstallment.Domain.ValueObjects;
using FluentValidation;

namespace EInstallment.Application.Validations.Members;

public sealed class UpdateMemberCommandValidator :
    AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(rule => rule.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
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