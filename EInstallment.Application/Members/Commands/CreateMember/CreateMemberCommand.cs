using EInstallment.Application.Abstractions.Messaging;

namespace EInstallment.Application.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email) : ICommand;