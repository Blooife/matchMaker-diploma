using System.Linq.Expressions;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Interfaces;

public interface IGenericRepository<T, TKey> where T : class
{
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
    Task<List<T>> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task DeleteManyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);
}