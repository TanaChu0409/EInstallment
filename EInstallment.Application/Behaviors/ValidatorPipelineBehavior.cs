﻿using EInstallment.Domain.Shared;
using FluentValidation;
using MediatR;

namespace EInstallment.Application.Behaviors;

public sealed class ValidatorPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next().ConfigureAwait(false);
        }

        Error[] errors = _validators
                            .Select(validator => validator.Validate(request))
                            .SelectMany(validationResult => validationResult.Errors)
                            .Where(validationFailure => validationFailure is not null)
                            .Select(failure => new Error(
                                failure.PropertyName,
                                failure.ErrorMessage))
                            .Distinct()
                            .ToArray();
        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next().ConfigureAwait(false);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        var validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}