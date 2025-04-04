using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Common;

namespace Opulenza.Infrastructure.Common.Persistence;

public class Repository<T> : IRepository<T> where T:BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _entitySet;
    
    public Repository(AppDbContext context) {
        _context = context;
        _entitySet = context.Set<T>();
    }
    
    public async Task<T?> GetByIdAsync(int id)
    {
       return await _entitySet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entitySet.ToListAsync();
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