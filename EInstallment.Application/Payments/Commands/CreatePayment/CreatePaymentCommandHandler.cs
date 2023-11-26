using EInstallment.Application.Abstractions.Messaging;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Application.Payments.Commands.CreatePayment;

internal sealed class CreatePaymentCommandHandler
    : ICommandHandler<CreatePaymentCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IInstallmentRepository _installmentRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IMemberRepository memberRepository,
        IInstallmentRepository installmentRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _installmentRepository = installmentRepository;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // get member by id from member repository
        var member = await _memberRepository.GetByIdAsync(
            request.CreatorId,
            cancellationToken)
            .ConfigureAwait(false);

        // if member not found return result error
        if (member is null)
        {
            return Result.Failure<Guid>(
                new Error(
                    "CreatePayment",
                    "Member not found."));
        }

        // get installment by id async from installment repository
        var installment = await _installmentRepository.GetByIdAsync(
            request.InstallmentId,
            cancellationToken)
            .ConfigureAwait(false);

        // if installment not found return result error
        if (installment is null)
        {
            return Result.Failure<Guid>(
                new Error(
                    "CreatePayment",
                    "Installment not found."));
        }

        // create payment
        var payment = Payment.Create(
            request.Amount,
            member,
            installment);
        // verify payment is return failure
        if (payment.IsFailure)
        {
            return Result.Failure<Guid>(payment.Error);
        }

        // add payment to payment repository
        _paymentRepository.Create(payment.Value);

        // save entities async
        await _unitOfWork
            .SaveEntitiesAsync(cancellationToken)
            .ConfigureAwait(false);

        // return result success
        return Result.Success(payment.Value.Id);
    }
}