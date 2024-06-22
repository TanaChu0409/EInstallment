using EInstallment.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
        .InstallServices(
            builder.Configuration,
            typeof(IServiceInstaller).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<EInstallment.Persistence.ApplicationDbContext>();
    await context.Database
        .MigrateAsync()
        .ConfigureAwait(false);
}

await app.RunAsync()
    .ConfigureAwait(false);