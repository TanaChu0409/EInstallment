using EInstallment.Application.Members.Commands.CreateMember;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.ValueObjects;
using Moq;

namespace EInstallment.Application.UnitTest.Members.Commands.CreateMember;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        var command = new CreateMemberCommand("John", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Member.EmailIsNotUnique, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        // Arrange
        var command = new CreateMemberCommand("John", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUnique()
    {
        // Arrange
        var command = new CreateMemberCommand("John", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        _memberRepositoryMock.Verify(x =>
            x.Create(
                It.Is<Member>(m => m.Id == result.Value),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenEmailIsNotUnique()
    {
        // Arrange
        var command = new CreateMemberCommand("John", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateMemberCommandHandler(
            _memberRepositoryMock.Object,
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