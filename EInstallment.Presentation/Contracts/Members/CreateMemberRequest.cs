namespace EInstallment.Api.Contracts.Members;

public sealed record CreateMemberRequest(
    string FirstName,
    string LastName,
    string Email);