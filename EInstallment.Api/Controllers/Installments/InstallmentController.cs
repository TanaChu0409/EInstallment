using EInstallment.Api.Abstractions;
using EInstallment.Application.Installments.Commands.CreateInstallment;
using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EInstallment.Api.Controllers.Installments;

[Route("api/[controller]")]
[ApiController]
public sealed class InstallmentController : ApiController
{
    public InstallmentController(ISender sender)
        : base(sender)
    {
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> CreateInstallment(
        [FromBody] CreateInstallmentRequest createInstallmentRequest,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateInstallmentCommand(
                createInstallmentRequest.ItemName,
                createInstallmentRequest.TotalNumberOfInstallment,
                createInstallmentRequest.TotalAmount,
                createInstallmentRequest.AmountOfEachInstallment,
                createInstallmentRequest.MemberId,
                createInstallmentRequest.CreditCardId),
                cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
            onSuccess: (value) => Ok(value),
            onFailure: (errorResult) => HandleFailure(errorResult));
    }
}