using EInstallment.Domain.Members;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EInstallment.Persistence.Repositories;

public sealed class MemberRepository : IMemberRepository
{
    private readonly EInstallmentContext _context;

    public MemberRepository(EInstallmentContext context)
    {
        _context = context;
    }

    public void Create(Member member)
    {
        _context.Set<Member>().Add(member);
    }

    public void Delete(Member member)
    {
        _context.Set<Member>().Remove(member);
    }

    public async Task<IReadOnlyCollection<Member>> GetAllMembersAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .AnyAsync(x => x.Email == email, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> IsEmailUniqueWithoutSelfAsync(Guid id, Email email, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .AnyAsync(x => x.Id != id &&
                           x.Email == email,
                      cancellationToken)
            .ConfigureAwait(false);
    }

    public void Update(Member member)
    {
        _context.Set<Member>().Update(member);
    }
}