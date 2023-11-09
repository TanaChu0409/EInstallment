using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Domain.Members;

public sealed class MemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateMemberAsync(
        FirstName firstName,
        LastName lastName,
        Email email,
        CancellationToken cancellationToken)
    {
        // Validate user email is unique
        if (!await _memberRepository
                    .IsEmailUniqueAsync(email, cancellationToken)
                    .ConfigureAwait(false))
        {
            return Result.Failure(DomainErrors.Member.EmailIsNotUnique);
        }

        // Create member
        var member = Member.Create(firstName, lastName, email, true);

        _memberRepository.Create(member.Value, cancellationToken);
        await _unitOfWork
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);
        return Result.Success();
    }
}