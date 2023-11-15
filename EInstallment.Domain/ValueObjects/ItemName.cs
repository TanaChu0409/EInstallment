using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class ItemName : ValueObject
{
    public const int MaxLength = 500;

    private ItemName(string itemName)
    {
        Value = itemName;
    }

    public string Value { get; }

    public static Result<ItemName> Create(string itemName) =>
        Result.Create(itemName)
                .Ensure(e =>
                    !string.IsNullOrWhiteSpace(e),
                    DomainErrors.ItemName.Empty)
                .Ensure(e =>
                    e.Length <= MaxLength,
                    DomainErrors.ItemName.OverSize)
                .Map(e =>
                    new ItemName(e));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}