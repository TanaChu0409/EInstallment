using EInstallment.Application.CreditCards.Commands.UpdateCreditCard;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace EInstallment.Application.UnitTest.CreditCards.Commands.UpdateCreditCard;

public class UpdateCreditCardCommandHandlerTests
{
    private readonly UpdateCreditCardCommandHandler _handler;
    private readonly ICreditCardRepository _creditCardRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly Guid Id = Guid.NewGuid();
    private const string UpdateCreditCardName = "玉山Ubear";
    private const int UpdatePaymentDay = 15;
    private static readonly CreditCardName creditCardName = CreditCardName.Create(UpdateCreditCardName).Value;

    public UpdateCreditCardCommandHandlerTests()
    {
        _creditCardRepositoryMock = Substitute.For<ICreditCardRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new(_creditCardRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCreditCardNotExist()
    {
        // Arrange
        var command = new UpdateCreditCardCommand(Id, UpdateCreditCardName, UpdatePaymentDay);

        _creditCardRepositoryMock
            .GetByIdAsync(Id, default)!
            .Returns(Task.FromResult<CreditCard>(null!)!);

        // Act
        var result = await _handler.Handle(command, default)
                            .ConfigureAwait(false);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(new Error(
                "EInstallment.UpdateCreditCard",
                "Credit card is not exist"));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCreditCardNameIsNotUniqueWithoutItSelf()
    {
        // Arrange
        var command = new UpdateCreditCardCommand(Id, UpdateCreditCardName, UpdatePaymentDay);
        var creditCard = CreditCard.Create(creditCardName, UpdatePaymentDay, true);
        _creditCardRepositoryMock
            .GetByIdAsync(Id, default)!
            .Returns(creditCard.Value);
        _creditCardRepositoryMock
            .IsCreditCardNameUniqueWithoutItSelfAsync(creditCard.Value.Id, creditCardName, default)
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, default)
                            .ConfigureAwait(false);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.CreditCard.CreditCardNameIsNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCreditCardNameIsUniqueWithoutItSelf()
    {
        // Arrange
        var command = new UpdateCreditCardCommand(Id, UpdateCreditCardName, UpdatePaymentDay);
        var creditCard = CreditCard.Create(creditCardName, UpdatePaymentDay, true);
        _creditCardRepositoryMock
            .GetByIdAsync(Id, default)!
            .Returns(creditCard.Value);
        _creditCardRepositoryMock
            .IsCreditCardNameUniqueWithoutItSelfAsync(creditCard.Value.Id, creditCardName, default)
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, default)
                            .ConfigureAwait(false);
        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallUpdateOnRepository_WhenCreditCardNameIsUniqueWithoutItSelf()
    {
        // Arrange
        var command = new UpdateCreditCardCommand(Id, UpdateCreditCardName, UpdatePaymentDay);
        var creditCard = CreditCard.Create(creditCardName, UpdatePaymentDay, true);
        _creditCardRepositoryMock
            .GetByIdAsync(Id, default)!
            .Returns(creditCard.Value);
        _creditCardRepositoryMock
            .IsCreditCardNameUniqueWithoutItSelfAsync(creditCard.Value.Id, creditCardName, default)
            .Returns(true);

        // Act
        _ = await _handler.Handle(command, default)
                            .ConfigureAwait(false);

        // Assert
        _creditCardRepositoryMock.Received(1)
            .Update(Arg.Is<CreditCard>(c =>
                        c.Id == creditCard.Value.Id &&
                        c.Name == creditCard.Value.Name &&
                        c.PaymentDay == creditCard.Value.PaymentDay));
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenCreditCardNameIsNotUniqueWithoutItSelf()
    {
        // Arrange
        var command = new UpdateCreditCardCommand(Id, UpdateCreditCardName, UpdatePaymentDay);
        var creditCard = CreditCard.Create(creditCardName, UpdatePaymentDay, true);
        _creditCardRepositoryMock
            .GetByIdAsync(Id, default)!
            .Returns(creditCard.Value);
        _creditCardRepositoryMock
            .IsCreditCardNameUniqueWithoutItSelfAsync(creditCard.Value.Id, creditCardName, default)
            .Returns(false);

        // Act
        _ = await _handler.Handle(command, default)
                            .ConfigureAwait(false);
        // Assert
        await _unitOfWorkMock.Received(0)
            .SaveEntitiesAsync(default)
            .ConfigureAwait(false);
    }
}