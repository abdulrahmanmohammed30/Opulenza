using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Infrastructure.Common.Persistence;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _entitySet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _entitySet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _entitySet.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entitySet.ToListAsync(cancellationToken);
    }

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }
}