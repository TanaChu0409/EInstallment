namespace EInstallment.Domain.Shared;

public sealed class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static implicit operator string(Error error) =>
        error.Code;

    public static bool operator ==(Error? left, Error? right) =>
        left is not null &&
        right is not null &&
        left.Equals(right);

    public static bool operator !=(Error? left, Error? right) =>
        !(left == right);

    public string Code { get; }

    public string Message { get; }

    public bool Equals(Error? other) =>
        other is not null &&
        other.Code == Code &&
        other.Message == Message;

    public override bool Equals(object? obj) =>
     obj is Error;

    public override int GetHashCode() =>
        HashCode.Combine(Code, Message);
}