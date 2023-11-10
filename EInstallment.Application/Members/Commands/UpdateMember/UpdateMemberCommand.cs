using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.Members.Commands.UpdateMember;
public sealed record UpdateMemberCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email) : ICommand;