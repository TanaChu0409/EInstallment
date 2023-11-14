using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class CreditCardName : ValueObject
{
    public const int MaxLength = 100;

    private CreditCardName(string creditCardName)
    {
        Value = creditCardName;
    }

    public string Value { get; }

    public static Result<CreditCardName> Create(string creditCardName) =>
        Result.Create(creditCardName)
                .Ensure(c =>
                        !string.IsNullOrWhiteSpace(c),
                        DomainErrors.CreditCardName.Empty)
                .Ensure(c =>
                        c.Length <= MaxLength,
                        DomainErrors.CreditCardName.OverSize)
                .Map(c =>
                     new CreditCardName(c));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}