using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EInstallment.Api.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
    internal readonly ISender Sender;

    protected ApiController(ISender sender) =>
        Sender = sender;

    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions =
            {
                { nameof(errors), errors },
            }
        };
}