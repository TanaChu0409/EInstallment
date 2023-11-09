using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.Members;

public interface IMemberRepository
{
    Task<Member> GetMemberById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Member>> GetAllMembersAsync(CancellationToken cancellationToken);

    void Create(Member member, CancellationToken cancellationToken);

    void Update(Member member, CancellationToken cancellationToken);

    void Delete(Member member, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);
}