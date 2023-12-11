using EInstallment.Api.Abstractions;
using EInstallment.Api.Contracts.Members;
using EInstallment.Application.Members.Commands.CreateMember;
using EInstallment.Application.Members.Commands.UpdateMember;
using EInstallment.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EInstallment.Api.Controllers.Members;

[Route("api/members")]
[ApiController]
public class MemberController : ApiController
{
    public MemberController(ISender sender)
        : base(sender)
    {
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateMemberAsync(
        [FromBody] CreateMemberRequest creatememberRequest,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateMemberCommand(
                creatememberRequest.FirstName,
                creatememberRequest.LastName,
                creatememberRequest.Email),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
                    onSuccess: (value) => Ok(value),
                    onFailure: (errorResult) => HandleFailure(errorResult));
    }

    [Route("")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateMemberAsync(
        [FromBody] UpdateMemberRequest updateMemberRequest,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new UpdateMemberCommand(
                updateMemberRequest.MemberId,
                updateMemberRequest.FirstName,
                updateMemberRequest.LastName,
                updateMemberRequest.Email),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
                        onSuccess: () => Ok(),
                        onFailure: (errorResult) => HandleFailure(errorResult));
    }
}