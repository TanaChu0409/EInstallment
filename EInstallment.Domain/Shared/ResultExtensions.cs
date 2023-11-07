namespace EInstallment.Domain.Shared;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mappingFunc) =>
        result.IsSuccess
        ? Result.Success(mappingFunc(result.Value))
        : Result.Failure<TOut>(result.Error);

    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ?
                onSuccess() :
                onFailure(result.Error);
    }
}