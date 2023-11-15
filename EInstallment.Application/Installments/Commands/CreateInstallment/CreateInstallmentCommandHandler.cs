using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Application.Installments.Commands.CreateInstallment;

internal sealed class CreateInstallmentCommandHandler : ICommandHandler<CreateInstallmentCommand, Guid>
{
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInstallmentCommandHandler(
        IInstallmentRepository installmentRepository,
        IMemberRepository memberRepository,
        ICreditCardRepository creditCardRepository,
        IUnitOfWork unitOfWork)
    {
        _installmentRepository = installmentRepository;
        _memberRepository = memberRepository;
        _creditCardRepository = creditCardRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<Result<Guid>> Handle(CreateInstallmentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}