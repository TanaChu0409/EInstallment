using EInstallment.Domain.SeedWork;
using EInstallment.Persistence;
using EInstallment.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Polly;
using Quartz;
using System.Text.Json;

namespace EInstallment.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync()
            .ConfigureAwait(false);

        foreach (OutboxMessage message in messages)
        {
            IDomainEvent? domainEvent = JsonSerializer
                .Deserialize<IDomainEvent>(message.Content);

            if (domainEvent is null)
            {
                continue;
            }

            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

            var result = await policy.ExecuteAndCaptureAsync(() =>
                _publisher.Publish(
                    domainEvent,
                    context.CancellationToken))
                .ConfigureAwait(false);

            message.Error = result.FinalException?.ToString();
            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}