using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.Members;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;

namespace EInstallment.Application.CreditCards.Commands.CreateMember;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:避免未具現化的內部類別", Justification = "<暫止>")]
internal sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var firstName = FirstName.Create(request.FirstName);

        if (firstName.IsFailure)
        {
            return Result.Failure(firstName.Error);
        }

        var lastName = LastName.Create(request.LastName);
        var email = Email.Create(request.Email);
        var isEmailUnique = await _memberRepository
                    .IsEmailUniqueAsync(email.Value, cancellationToken)
                    .ConfigureAwait(false);

        // Create member
        var member = Member.Create(
                        firstName.Value,
                        lastName.Value,
                        email.Value,
                        isEmailUnique);

        _memberRepository.Create(member.Value, cancellationToken);
        await _memberRepository
                .SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

        return Result.Success();
    }
}