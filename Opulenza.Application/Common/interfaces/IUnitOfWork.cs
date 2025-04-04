namespace Opulenza.Application.Common.interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
}