﻿using EInstallment.Domain.Members;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Persistence.Repositories;

public sealed class MemberRepository : IMemberRepository
{
    public void Create(Member member, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Delete(Member member, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Member>> GetAllMembersAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Member?> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueWithoutSelfAsync(Guid id, Email email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Update(Member member, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}