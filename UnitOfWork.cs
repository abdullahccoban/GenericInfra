using System.Collections;
using GenericInfra.Core;
using Microsoft.EntityFrameworkCore;

namespace GenericInfra;

public class UnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;
    private Hashtable _repositories;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }
    
    public IRepository<T> GetRepository<T>() where T : class, IEntity
    {
        if (_repositories == null) 
        {
            _repositories = new Hashtable();
        }
        
        var typeName = typeof(T).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryInstance = new GenericRepository<T, TContext>(_context);
            _repositories.Add(typeName, repositoryInstance);
        }
        
        return (IRepository<T>)_repositories[typeName];
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}