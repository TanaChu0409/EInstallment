namespace EInstallment.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:屬性不應傳回陣列", Justification = "<暫止>")]
    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) =>
        new(errors);
}