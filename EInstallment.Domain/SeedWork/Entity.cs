namespace EInstallment.Domain.SeedWork;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }

    public static bool operator ==(Entity? left, Entity? right) =>
        left is not null && right is not null && left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) =>
        !(left == right);

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 31;
    }
}