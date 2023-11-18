using EInstallment.Api.Abstractions;
using EInstallment.Application.Installments.Commands.CreateInstallment;
using EInstallment.Application.Installments.Commands.UpdateInstallment;
using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateInstallmentAsync(
        [FromBody] CreateInstallmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateInstallmentCommand(
                request.ItemName,
                request.TotalNumberOfInstallment,
                request.TotalAmount,
                request.AmountOfEachInstallment,
                request.MemberId,
                request.CreditCardId),
                cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
            onSuccess: (value) => Ok(value),
            onFailure: (errorResult) => HandleFailure(errorResult));
    }

    [Route("")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateInstallmentAsync(
        [FromBody] UpdateInstallmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new UpdateInstallmentCommand(
                request.InstallmentId,
                request.ItemName,
                request.TotalNumberOfInstallment,
                request.TotalAmount,
                request.AmountOfEachInstallment),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
            onSuccess: () => Ok(),
            onFailure: (errorResult) => HandleFailure(errorResult));
    }
}