using EInstallment.Domain.SeedWork;

namespace EInstallment.Domain.Members;

public sealed class Member : Entity
{
    private Member(
        Guid id,
        string firstName,
        string lastName,
        string email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreateOnUtc = DateTime.UtcNow;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public DateTime CreateOnUtc { get; private set; }

    public static Member Create(
        string firstName,
        string lastName,
        string email)
    {
        var member = new Member(
            Guid.NewGuid(),
            firstName,
            lastName,
            email);

        return member;
    }

    public void Update(
        string firstName,
        string lastName,
        string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}