namespace EInstallment.Domain.Shared;

public sealed class ValidationResult<TValue> :
    Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:屬性不應傳回陣列", Justification = "<暫止>")]
    public Error[] Errors { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:請勿在泛型型別上宣告靜態成員", Justification = "<暫止>")]
    public static ValidationResult<TValue> WithErrors(Error[] errors) =>
        new(errors);
}