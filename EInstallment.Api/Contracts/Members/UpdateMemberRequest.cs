namespace EInstallment.Api.Contracts.Members;

public sealed record UpdateMemberRequest(
    Guid MemberId,
    string FirstName,
    string LastName,
    string Email);