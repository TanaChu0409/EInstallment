using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.Members.Commands.CreateMember;

internal sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMemberCommandHandler(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var firstName = FirstName.Create(request.FirstName);
        if (firstName.IsFailure)
        {
            return Result.Failure<Guid>(firstName.Error);
        }

        var lastName = LastName.Create(request.LastName);
        if (lastName.IsFailure)
        {
            return Result.Failure<Guid>(lastName.Error);
        }

        var email = Email.Create(request.Email);
        if (email.IsFailure)
        {
            return Result.Failure<Guid>(email.Error);
        }

        var isEmailUnique = await _memberRepository
                    .IsEmailUniqueAsync(email.Value, cancellationToken)
                    .ConfigureAwait(false);

        // Create member
        var member = Member.Create(
                        firstName.Value,
                        lastName.Value,
                        email.Value,
                        isEmailUnique);

        if (member.IsFailure)
        {
            return Result.Failure<Guid>(member.Error);
        }

        _memberRepository.Create(member.Value);
        await _unitOfWork
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

        return Result.Success(member.Value.Id);
    }
}