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
        var command = new CreateMemberCommand("first name", "last name", "email@test.com");

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
}