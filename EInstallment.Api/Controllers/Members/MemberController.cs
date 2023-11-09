using EInstallment.Api.Abstractions;
using EInstallment.Application.Members.Commands.CreateMember;
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> CreateMemberAsync([FromBody] CreateMemberRequest CreatememberRequest, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreateMemberCommand(
                CreatememberRequest.FirstName,
                CreatememberRequest.LastName,
                CreatememberRequest.Email),
            cancellationToken)
            .ConfigureAwait(false);

        return result.Match(
                    onSuccess: (value) => Results.Ok(value),
                    onFailure: (error) => Results.BadRequest(error));
    }
}