using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.Members;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Member>> GetAllMembersAsync(CancellationToken cancellationToken);

    void Create(Member member);

    void Update(Member member);

    void Delete(Member member);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueWithoutSelfAsync(Guid id, Email email, CancellationToken cancellationToken);
}