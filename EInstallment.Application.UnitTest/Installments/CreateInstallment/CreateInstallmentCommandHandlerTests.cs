using EInstallment.Application.Installments.Commands.CreateInstallment;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EInstallment.Application.UnitTest.Installments.CreateInstallment;

public class CreateInstallmentCommandHandlerTests
{
    private readonly CreateInstallmentCommandHandler _handler;
    private readonly IInstallmentRepository _installmentRepositoryMock;
    private readonly IMemberRepository _memberRepositoryMock;
    private readonly ICreditCardRepository _creditCardRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private readonly string _itemName;
    private readonly int _totalNumberOfInstallment;
    private readonly decimal _totalAmount;
    private readonly decimal _amountOfEachInstallment;
    private readonly Guid _memberId;
    private readonly Guid _creditCardId;
    private readonly Member _member;
    private readonly CreditCard _creditCard;

    private readonly ItemName _itemNameVO;
    private readonly FirstName _firstNameVO;
    private readonly LastName _lastNameVO;
    private readonly Email _emailVO;
    private readonly CreditCardName _creditCardNameVO;

    public CreateInstallmentCommandHandlerTests()
    {
        _installmentRepositoryMock = Substitute.For<IInstallmentRepository>();
        _memberRepositoryMock = Substitute.For<IMemberRepository>();
        _creditCardRepositoryMock = Substitute.For<ICreditCardRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new(
            _installmentRepositoryMock,
            _memberRepositoryMock,
            _creditCardRepositoryMock,
            _unitOfWorkMock);

        _itemName = "ROG G615JV";
        _totalNumberOfInstallment = 30;
        _totalAmount = 59999m;
        _amountOfEachInstallment = 1999m;
        _memberId = Guid.NewGuid();
        _creditCardId = Guid.NewGuid();
        _firstNameVO = FirstName.Create("Tana").Value;
        _lastNameVO = LastName.Create("Chu").Value;
        _emailVO = Email.Create("sunnychu1995@gmail.com").Value;
        _creditCardNameVO = CreditCardName.Create("Citi Bank Cash Back Plus").Value;
        _itemNameVO = ItemName.Create(_itemName).Value;

        _member = Member.Create(_firstNameVO, _lastNameVO, _emailVO, true).Value;
        _creditCard = CreditCard.Create(_creditCardNameVO, 5, true).Value;
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCreatorIsNotExist()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(new Error(
                "EInstallment.CreateInstallmentHandler",
                $"The member id {_memberId} is not exist"));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCreditCardIsNotExist()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(Task.FromResult<CreditCard>(null!));

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(new Error(
                "EInstallment.CreateInstallmentHandler",
                $"The credit card id {_creditCardId} is not exist"));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenTotalNumberOfInstallmentIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            0,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Installment.TotalNumberOfInstallmentLessThanOne);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenTotalAmountIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            0,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Installment.TotalAmountLessThanOne);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenAmountOfEachInstallmeIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            0,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Installment.AmountOfEachInstallmentLessThanOne);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenAllConditionIsSatisfaction()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
             .GetByIdAsync(_memberId, default)!
             .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallOnCreateRepository_WhenAllConditionIsSatisfaction()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
             .GetByIdAsync(_memberId, default)!
             .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        _installmentRepositoryMock.Received(1)
            .Create(Arg.Is<Installment>(i => i.Id == result.Value &&
                                             i.ItemName == _itemNameVO &&
                                             i.TotalNumberOfInstallment == _totalNumberOfInstallment &&
                                             i.TotalAmount == _totalAmount &&
                                             i.AmountOfEachInstallment == _amountOfEachInstallment &&
                                             i.Creator == _member &&
                                             i.CreditCard == _creditCard));
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenCreatorIsNotExist()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(Task.FromResult<Member>(null!));

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
                .Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenCreditCardIsNotExist()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(Task.FromResult<CreditCard>(null!));

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
                .Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenTotalNumberOfInstallmentIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            0,
            _totalAmount,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
                .Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenTotalAmountIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            0,
            _amountOfEachInstallment,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
                .Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenAmountOfEachInstallmeIsZero()
    {
        // Arrange
        var command = new CreateInstallmentCommand(
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            0,
            _memberId,
            _creditCardId);

        _memberRepositoryMock
            .GetByIdAsync(_memberId, default)!
            .Returns(_member);

        _creditCardRepositoryMock
            .GetByIdAsync(_creditCardId, default)!
            .Returns(_creditCard);

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock
                .Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }
}