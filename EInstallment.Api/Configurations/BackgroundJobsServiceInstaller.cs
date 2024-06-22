using EInstallment.Infrastructure.BackgroundJobs;
using Quartz;

namespace EInstallment.Api.Configurations;

public sealed class BackgroundJobsServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            config
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(100).RepeatForever()));
        });

        services.AddQuartzHostedService();
    }
}