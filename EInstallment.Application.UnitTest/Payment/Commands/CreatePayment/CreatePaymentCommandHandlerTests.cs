using EInstallment.Application.Payments.Commands.CreatePayment;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.Payments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EInstallment.Application.UnitTest.Payment.Commands.CreatePayment;

public class CreatePaymentCommandHandlerTests
{
    private readonly CreatePaymentCommandHandler _handler;
    private readonly IMemberRepository _memberRepositoryMock;
    private readonly IInstallmentRepository _installmentRepositoryMock;
    private readonly IPaymentRepository _paymentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private readonly decimal _amount;
    private readonly Guid _creatorId;
    private readonly Guid _installmentId;
    private readonly Member _member;
    private readonly Installment _installment;

    public CreatePaymentCommandHandlerTests()
    {
        _memberRepositoryMock = Substitute.For<IMemberRepository>();
        _installmentRepositoryMock = Substitute.For<IInstallmentRepository>();
        _paymentRepositoryMock = Substitute.For<IPaymentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new(
            _memberRepositoryMock,
            _installmentRepositoryMock,
            _paymentRepositoryMock,
            _unitOfWorkMock);

        _amount = 59999m;
        _creatorId = Guid.NewGuid();
        _installmentId = Guid.NewGuid();
        _member = Member.Create(
            FirstName.Create("John").Value,
            LastName.Create("Doe").Value,
            Email.Create("john.doe@test.com").Value,
            true).Value;
        var creditCard = CreditCard.Create(
            CreditCardName.Create("Citi Bank Cash Back Plus").Value,
            5,
            true).Value;
        _installment = Installment.Create(
            ItemName.Create("ROG G615JV").Value,
            30,
            59999m,
            1999m,
            _member,
            creditCard).Value;
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenMemberNotFound()
    {
        // Arrange
        var command = new CreatePaymentCommand(
                        _amount,
                        _creatorId,
                        _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(
            new Error(
                "CreatePayment",
                "Member not found."));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenInstallmentNotFound()
    {
        // Arrange
        var command = new CreatePaymentCommand(_amount, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(
            new Error(
                "CreatePayment",
                "Installment not found."));
    }

    [Fact]
    public async Task Handler_Should_ReturnFailureResult_WhenAmountIsLessThanOne()
    {
        // Arrange
        var command = new CreatePaymentCommand(0m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(
            new Error(
                "CreatePayment",
                "Amount must be greater than zero."));
    }

    [Fact]
    public async Task Handler_Should_ReturnFailureResult_WhenAmountIsGreaterThanInstallmentAmount()
    {
        // Arrange
        var command = new CreatePaymentCommand(60000m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(DomainErrors.Payment.AmountGreaterThanInstallmentAmount);
    }

    [Fact]
    public async Task Handler_Should_NotCallUnitOfWork_WhenMemberNotFound()
    {
        // Arrange
        var command = new CreatePaymentCommand(
                        _amount,
                        _creatorId,
                        _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
            .DidNotReceive()
            .SaveEntitiesAsync(default)
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handler_Should_NotCallUnitOfWork_WhenAmountIsLessThanOne()
    {
        // Arrange
        var command = new CreatePaymentCommand(0m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
            .DidNotReceive()
            .SaveEntitiesAsync(default)
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handler_Should_NotCallUnitOfWork_WhenAmountIsGreaterThanInstallmentAmount()
    {
        // Arrange
        var command = new CreatePaymentCommand(60000m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
            .DidNotReceive()
            .SaveEntitiesAsync(default)
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccessResult_WhenAllConditionIsSatisfaction()
    {
        // Arrange
        var command = new CreatePaymentCommand(1999m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_CallCreateOnRepository_WhenAllConditionIsSatisfaction()
    {
        // Arrange
        var command = new CreatePaymentCommand(1999m, _creatorId, _installmentId);

        _memberRepositoryMock
            .GetByIdAsync(_creatorId, default)
            .Returns(_member);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)
            .Returns(_installment);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        _paymentRepositoryMock
            .Received(1)
            .Create(Arg.Is<Domain.Payments.Payment>(p => p.Id == result.Value));
    }
}