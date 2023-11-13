using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.Members.Commands.UpdateMember;

internal sealed class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberCommandHandler(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var memeber = await _memberRepository
                            .GetMemberByIdAsync(request.Id, cancellationToken)
                            .ConfigureAwait(false);

        if (memeber is null)
        {
            return Result.Failure(new Error(
                "EInstallment.UpdateMember",
                "Member is not exist"));
        }

        var firstName = FirstName.Create(request.FirstName);
        if (firstName.IsFailure)
        {
            return Result.Failure(firstName.Error);
        }

        var lastName = LastName.Create(request.LastName);
        if (lastName.IsFailure)
        {
            return Result.Failure(lastName.Error);
        }

        var email = Email.Create(request.Email);
        if (email.IsFailure)
        {
            return Result.Failure(email.Error);
        }

        var isEmailUniqueWithoutSelf = await _memberRepository
                    .IsEmailUniqueWithoutSelfAsync(
                        memeber.Id,
                        email.Value,
                        cancellationToken)
                    .ConfigureAwait(false);

        var result = memeber.Update(
                        firstName.Value,
                        lastName.Value,
                        email.Value,
                        isEmailUniqueWithoutSelf);

        if (result.IsFailure)
        {
            return result;
        }

        _memberRepository.Update(memeber, cancellationToken);
        await _unitOfWork
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

        return Result.Success();
    }
}