using NetArchTest.Rules;
using System.Reflection;

namespace EInstallment.ArchitectureTest;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:識別項不應包含底線", Justification = "<暫止>")]
public class ArchitectureTests
{
    private readonly string _presentationNameSpace = Api.AssemblyReference.Namespace;
    private readonly string _applicationNameSpace = Application.AssemblyReference.Namespace;

    private static Assembly DomainAssembly =>
        Domain.AssemblyReference.Assembly;

    private static Assembly ApplicationAssembly =>
        Application.AssemblyReference.Assembly;

    private static Assembly PersistenceAssembly =>
        Persistence.AssemblyReference.Assembly;

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var types = Types.InAssembly(DomainAssembly);
        var otherProjects = new[]
        {
            _applicationNameSpace,
            _presentationNameSpace,
        };
        // Act
        var testResult = types.ShouldNot()
                                .HaveDependencyOnAll(otherProjects)
                                .GetResult();
        // Assert
        Assert.True(testResult.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var types = Types.InAssembly(ApplicationAssembly);
        var otherProjects = new[]
        {
            _presentationNameSpace,
        };
        // Act
        var testResult = types.ShouldNot()
                             .HaveDependencyOnAll(otherProjects)
                             .GetResult();
        // Assert
        Assert.True(testResult.IsSuccessful);
    }

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var types = Types.InAssembly(PersistenceAssembly);
        var otherProjects = new[]
        {
            _presentationNameSpace
        };

        // Act
        var testResult = types.ShouldNot()
                            .HaveDependencyOnAll(otherProjects)
                            .GetResult();
        // Assert
        Assert.True(testResult.IsSuccessful);
    }
}