using EInstallment.Api.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
        .InstallServices(
            builder.Configuration,
            typeof(IServiceInstaller).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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