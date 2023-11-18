using EInstallment.Application.Installments.Commands.UpdateInstallment;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Installments;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EInstallment.Application.UnitTest.Installments.UpdateInstallment;

public class UpdateInstallmentCommandHandlerTests
{
    private readonly UpdateInstallmentCommandHandler _handler;
    private readonly IInstallmentRepository _installmentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private readonly Guid _installmentId;
    private readonly string _itemName;
    private readonly int _totalNumberOfInstallment;
    private readonly decimal _totalAmount;
    private readonly decimal _amountOfEachInstallment;

    private readonly ItemName _itemNameVO;

    private readonly Installment _installmentMock;

    public UpdateInstallmentCommandHandlerTests()
    {
        _installmentRepositoryMock = Substitute.For<IInstallmentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new(
            _installmentRepositoryMock,
            _unitOfWorkMock);

        _installmentId = Guid.NewGuid();
        _itemName = "ROG G615JV";
        _totalNumberOfInstallment = 30;
        _totalAmount = 59999m;
        _amountOfEachInstallment = 1999m;
        _itemNameVO = ItemName.Create(_itemName).Value;

        _installmentMock = Installment.Create(
            _itemNameVO,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment,
            null!,
            null!).Value;
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenInstallmentIdIsNotExist()
    {
        // Arrange
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .ReturnsNull();
        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(new Error(
                "EInstallment.UpdateInstallmentHandler",
                "Can't find this installment"));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenTotalNumberOfInstallmentIsZero()
    {
        // Arrange
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            0,
            _totalAmount,
            _amountOfEachInstallment);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .Returns(_installmentMock);

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
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            _totalNumberOfInstallment,
            0,
            _amountOfEachInstallment);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .Returns(_installmentMock);

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
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            0);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .Returns(_installmentMock);

        // Act
        var result = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Installment.AmountOfEachInstallmentLessThanOne);
    }

    [Fact]
    public async Task Handle_Should_CallOnUpdateRepository_WhenAllConditionIsSatisfaction()
    {
        // Arrange
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .Returns(_installmentMock);

        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        _installmentRepositoryMock.Received(1)
            .Update(Arg.Is<Installment>(i => i.ItemName.Value == _itemName &&
                                             i.TotalNumberOfInstallment == _totalNumberOfInstallment &&
                                             i.TotalAmount == _totalAmount &&
                                             i.AmountOfEachInstallment == _amountOfEachInstallment &&
                                             i.Status == InstallmentStatus.Upcoming));
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenInstallmentIdIsNotExist()
    {
        // Arrange
        var command = new UpdateInstallmentCommand(
            _installmentId,
            _itemName,
            _totalNumberOfInstallment,
            _totalAmount,
            _amountOfEachInstallment);

        _installmentRepositoryMock
            .GetByIdAsync(_installmentId, default)!
            .ReturnsNull();
        // Act
        _ = await _handler.Handle(command, default).ConfigureAwait(false);

        // Assert
        await _unitOfWorkMock.Received(0)
                .SaveEntitiesAsync(default)
                .ConfigureAwait(false);
    }
}