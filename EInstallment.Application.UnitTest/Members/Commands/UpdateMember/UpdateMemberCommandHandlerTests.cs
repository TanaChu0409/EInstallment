using EInstallment.Application.Members.Commands.UpdateMember;
using EInstallment.Domain.Errors;
using EInstallment.Domain.Members;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;
using EInstallment.Domain.ValueObjects;
using Moq;

namespace EInstallment.Application.UnitTest.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenMemberNotExist()
    {
        // Arrange
        var mockMemberId = Guid.NewGuid();
        var command = new UpdateMemberCommand(mockMemberId, "Jane", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.GetMemberByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Member>(null!)!);

        var handler = new UpdateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(new Error(
                "EInstallment.UpdateMember",
                "Member is not exist"),
                result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnqueWithoutSelf()
    {
        // Arrange
        var mockMemberId = Guid.NewGuid();
        var mockMember = Member.Create(
            FirstName.Create("AA").Value,
            LastName.Create("CC").Value,
            Email.Create("john.doe@test.com").Value,
            true).Value;

        var command = new UpdateMemberCommand(mockMemberId, "Jane", "Doe", "john.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.GetMemberByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockMember)!);
        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueWithoutSelfAsync(
                It.IsAny<Guid>(),
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new UpdateMemberCommandHandler(
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
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUniqueWithoutSelf()
    {
        // Arrange
        var mockMemberId = Guid.NewGuid();
        var mockMember = Member.Create(
            FirstName.Create("AA").Value,
            LastName.Create("CC").Value,
            Email.Create("john.doe@test.com").Value,
            true).Value;

        var command = new UpdateMemberCommand(mockMemberId, "Jane", "Doe", "jane.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.GetMemberByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockMember)!);
        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueWithoutSelfAsync(
                It.IsAny<Guid>(),
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new UpdateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);
        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_CallUpdateOnRepository_WhenEmailIsUniqueWithoutSelf()
    {
        // Arrange
        var mockMemberId = Guid.NewGuid();
        var mockMember = Member.Create(
            FirstName.Create("AA").Value,
            LastName.Create("CC").Value,
            Email.Create("john.doe@test.com").Value,
            true).Value;

        var command = new UpdateMemberCommand(mockMemberId, "Jane", "Doe", "jane.doe@test.com");

        _memberRepositoryMock.Setup(x =>
            x.GetMemberByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockMember)!);

        _memberRepositoryMock.Setup(x =>
            x.IsEmailUniqueWithoutSelfAsync(
                It.IsAny<Guid>(),
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new UpdateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, default)
                                    .ConfigureAwait(false);

        // Assert
        _memberRepositoryMock.Verify(x =>
            x.Update(
                It.Is<Member>(m => m.Id == mockMemberId),
                It.IsAny<CancellationToken>()),
                Times.Once);
    }
}