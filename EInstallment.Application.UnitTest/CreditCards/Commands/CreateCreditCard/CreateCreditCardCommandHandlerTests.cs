using EInstallment.Application.CreditCards.Commands.CreateCreditCard;
using EInstallment.Domain.CreditCards;
using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace EInstallment.Application.UnitTest.CreditCards.Commands.CreateCreditCard;

public class CreateCreditCardCommandHandlerTests
{
    private readonly Mock<ICreditCardRepository> _creditCardRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private static string CreditCardNameText = "國泰Cube";
    private static int PaymentDay = 6;

    public CreateCreditCardCommandHandlerTests()
    {
        _creditCardRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handler_Should_ReturnFailureResult_WhenCreditCardNameIsNotUnique()
    {
        // Arrange
        var command = new CreateCreditCardCommand(CreditCardNameText, PaymentDay);

        _creditCardRepositoryMock.Setup(x =>
            x.IsCreditCardNameUniqueAsync(
                It.IsAny<CreditCardName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateCreditCardCommandHandler(
            _creditCardRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        result.Error
                .Should()
                .Be(DomainErrors.CreditCard.CreditCardNameIsNotUnique);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccessResult_WhenCreditCardNameIsUnique()
    {
        // Arrange
        var command = new CreateCreditCardCommand(CreditCardNameText, PaymentDay);

        _creditCardRepositoryMock.Setup(x =>
            x.IsCreditCardNameUniqueAsync(
                It.IsAny<CreditCardName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateCreditCardCommandHandler(
            _creditCardRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_CallCreateOnRepository_WhenCreditCardNameIsUnique()
    {
        // Arrange
        var command = new CreateCreditCardCommand(CreditCardNameText, PaymentDay);

        _creditCardRepositoryMock.Setup(x =>
            x.IsCreditCardNameUniqueAsync(
                It.IsAny<CreditCardName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateCreditCardCommandHandler(
            _creditCardRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);

        // Assert
        _creditCardRepositoryMock.Verify(x =>
            x.Create(
                It.Is<CreditCard>(c => c.Id == result.Value),
                It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task Handler_Should_NotCallUnitOfWork_WhenCreditCardNameIsNotUnique()
    {
        // Arrange
        var command = new CreateCreditCardCommand(CreditCardNameText, PaymentDay);

        _creditCardRepositoryMock.Setup(x =>
            x.IsCreditCardNameUniqueAsync(
                It.IsAny<CreditCardName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateCreditCardCommandHandler(
            _creditCardRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        _ = await handler.Handle(command, default)
                                    .ConfigureAwait(false);

        // Assert
        _unitOfWorkMock.Verify(x =>
            x.SaveEntitiesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}