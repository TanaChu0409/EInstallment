using Scrutor;

namespace EInstallment.Api.Configurations;

public class PersistenceServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(
                    Persistence.AssemblyReference.Assembly)
                .AddClasses(false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());
    }
}