using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 100;

    private LastName(string lastName)
    {
        Value = lastName;
    }

    public string Value { get; }

    public static Result<LastName> Create(string lastName) =>
        Result.Create(lastName)
                .Ensure(l =>
                        !string.IsNullOrWhiteSpace(l),
                        DomainErrors.LastName.Empty)
                .Ensure(l =>
                        l.Length <= MaxLength,
                        DomainErrors.LastName.OverSize)
                .Map(l =>
                     new LastName(l));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}