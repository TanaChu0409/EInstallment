namespace EInstallment.Domain.SeedWork;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "<暫止>")]
public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left is not null && right is not null && left.Equals(right);

    public static bool operator !=(ValueObject? left, ValueObject? right) =>
        !(left == right);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together", Justification = "<暫止>")]
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