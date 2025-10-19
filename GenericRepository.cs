using GenericInfra.Core;
using Microsoft.EntityFrameworkCore;

namespace GenericInfra;

public class GenericRepository<T, TContext> : IRepository<T>
    where T : class, IEntity
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}