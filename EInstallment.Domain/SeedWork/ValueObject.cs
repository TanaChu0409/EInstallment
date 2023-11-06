namespace EInstallment.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left is not null && right is not null && left.Equals(right);

    public static bool operator !=(ValueObject? left, ValueObject? right) =>
        !(left == right);

    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object? obj) =>
        obj is ValueObject other && ValuesAreEqual(other);

    public override int GetHashCode() =>
        GetAtomicValues()
                .Aggregate(default(int), HashCode.Combine);

    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValues()
                .SequenceEqual(other.GetAtomicValues());
    }
}