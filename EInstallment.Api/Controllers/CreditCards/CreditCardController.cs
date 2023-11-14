using EInstallment.Api.Abstractions;
using EInstallment.Application.CreditCards.Commands.CreateCreditCard;
using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EInstallment.Api.Controllers.CreditCards;

[Route("api/credit-cards")]
[ApiController]
public sealed class CreditCardController : ApiController
{
    public CreditCardController(ISender sender)
        : base(sender)
    {
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateCreditCardAsync(
        [FromBody] CreateCreditCardRequest createCreditCardRequest,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateCreditCardCommand(
                createCreditCardRequest.CreditCardName,
                createCreditCardRequest.PaymentDay),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
            onSuccess: (value) => Ok(value),
            onFailure: (errorResult) => HandleFailure(errorResult));
    }
}