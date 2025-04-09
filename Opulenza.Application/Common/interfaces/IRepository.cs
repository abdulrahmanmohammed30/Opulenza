namespace Opulenza.Application.Common.interfaces;

public interface IRepository<T> where T:class, IEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
