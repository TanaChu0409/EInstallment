namespace EInstallment.Api.Controllers.Members;

public sealed record UpdateMemberRequest(
    Guid MemberId,
    string FirstName,
    string LastName,
    string Email);