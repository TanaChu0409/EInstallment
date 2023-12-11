using EInstallment.Api.Abstractions;
using EInstallment.Api.Contracts.Payments;
using EInstallment.Application.Payments.Commands.CreatePayment;
using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EInstallment.Api.Controllers.Payments;

[Route("api/payments")]
[ApiController]
public class PaymentController : ApiController
{
    public PaymentController(ISender sender)
        : base(sender)
    {
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreatePaymentAsync(
            [FromBody] CreatePaymentRequest createPaymentRequest,
            CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(
            new CreatePaymentCommand(
                createPaymentRequest.Amount,
                createPaymentRequest.CreatorId,
                createPaymentRequest.InstallmentId),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
            onSuccess: (value) => Ok(value),
            onFailure: (errorResult) => HandleFailure(errorResult));
    }
}