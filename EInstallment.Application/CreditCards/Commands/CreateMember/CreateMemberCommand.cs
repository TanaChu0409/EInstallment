﻿namespace EInstallment.Application.CreditCards.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email);