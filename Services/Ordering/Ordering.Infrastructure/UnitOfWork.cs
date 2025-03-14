using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Contracts;
using Polly;

namespace Ordering.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(OrderDbContext orderDbContext,ILogger<UnitOfWork> logger)
    {
        _orderDbContext = orderDbContext;
        _logger = logger;
    }
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _orderDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteOptimisticUpdateAsync(Func<Task> task)
    {
        int retries = UnitOfWorkConfig.OptimisticConcurrencyConflictRetryCount;
        var retryPolicy = Policy.Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(retries,
            (attempt) => TimeSpan.FromMicroseconds(10 * Math.Pow(2, attempt)),
            (ex, timespan, context) =>
            {
                _logger.LogWarning(ex, $"A conflict occured during the optimistic update. Retrying the update after {timespan.Milliseconds}ms.");
                ResetContext();
            });

        try
        {
            await retryPolicy.ExecuteAsync(task);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "A conflict occured during the optimistic update. Exceeded max retry count.");
            throw;
        }
    }

    private void ResetContext()
    {
        _orderDbContext.ChangeTracker.Clear();
    }

}
