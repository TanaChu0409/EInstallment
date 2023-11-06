namespace EInstallment.Domain.Members;

public interface IMemberRepository
{
    Task<Member> GetMemberById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Member>> GetAllMembersAsync(CancellationToken cancellationToken);

    Task Create(Member member, CancellationToken cancellationToken);

    Task Update(Member member, CancellationToken cancellationToken);

    Task Delete(Member member, CancellationToken cancellationToken);
}