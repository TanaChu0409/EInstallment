namespace EInstallment.Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:屬性不應傳回陣列", Justification = "<暫止>")]
    Error[] Errors { get; }
}