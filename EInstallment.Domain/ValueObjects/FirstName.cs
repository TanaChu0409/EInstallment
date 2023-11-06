using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 100;

    private FirstName(string firstName)
    {
        Value = firstName;
    }

    public string Value { get; }

    public static Result<FirstName> Create(string firstName) =>
        Result.Create(firstName)
                .Ensure(f =>
                        !string.IsNullOrWhiteSpace(f),
                        DomainErrors.FirstName.Empty)
                .Ensure(f =>
                        f.Length <= MaxLength,
                        DomainErrors.FirstName.OverSize)
                .Map(f =>
                     new FirstName(f));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}