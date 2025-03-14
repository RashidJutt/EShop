namespace Ordering.Core.Contracts;

public interface IUnitOfWork
{
    Task ExecuteOptimisticUpdateAsync(Func<Task> task);
    Task CommitAsync(CancellationToken cancellationToken = default);
}
