using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.Members;

public sealed class Member : Entity
{
    private Member()
    {
    }

    private Member(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreateOnUtc = DateTime.UtcNow;
    }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public static Result<Member> Create(
        FirstName firstName,
        LastName lastName,
        Email email,
        bool isEmailUnique)
    {
        // Validate user email is unique
        if (!isEmailUnique)
        {
            return Result.Failure<Member>(DomainErrors.Member.EmailIsNotUnique);
        }

        var member = new Member(
            Guid.NewGuid(),
            firstName,
            lastName,
            email);

        return member;
    }

    public Result Update(
        FirstName firstName,
        LastName lastName,
        Email email,
        bool isEmailUnique)
    {
        // Validate user email is unique
        if (!isEmailUnique)
        {
            return Result.Failure(DomainErrors.Member.EmailIsNotUnique);
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;

        return Result.Success();
    }
}