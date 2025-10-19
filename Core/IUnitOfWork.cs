namespace GenericInfra.Core;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class, IEntity;
    Task<int> SaveChangesAsync();
}