namespace EInstallment.Api.Controllers.Members;

public sealed record CreateMemberRequest(
    string FirstName,
    string LastName,
    string Email);