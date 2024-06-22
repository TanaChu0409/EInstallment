using Microsoft.EntityFrameworkCore;

namespace EInstallment.Api.Configurations;

public sealed class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
             .Scan(
                 selector => selector
                     .FromAssemblies(
                         Infrastructure.AssemblyReference.Assembly,
                         Persistence.AssemblyReference.Assembly)
                     .AddClasses(false)
                     .AsImplementedInterfaces()
                     .WithScopedLifetime());

        services.AddSingleton<Persistence.Interceptors.ConvertDomainEventsToOutboxMessageInterceptor>();
        services.AddDbContext<Persistence.ApplicationDbContext>(
            optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString("Database")!,
                    builder => builder.MigrationsAssembly("EInstallment.Api"));
            });
    }
}