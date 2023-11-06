using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class BankName : ValueObject
{
    public const int MaxLength = 100;

    private BankName(string bankName)
    {
        Value = bankName;
    }

    public string Value { get; }

    public static Result<BankName> Create(string bankName) =>
        Result.Create(bankName)
                .Ensure(bank =>
                        !string.IsNullOrWhiteSpace(bank),
                        DomainErrors.BankName.Empty)
                .Ensure(bank =>
                        bank.Length <= MaxLength,
                        DomainErrors.BankName.OverSize)
                .Map(bank =>
                     new BankName(bank));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}