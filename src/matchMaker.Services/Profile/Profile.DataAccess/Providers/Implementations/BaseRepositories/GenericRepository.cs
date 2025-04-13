using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Profile.DataAccess.Interfaces.BaseRepositories;
using Profile.DataAccess.Models;
using Profile.DataAccess.Specifications;
using Profile.DataAccess.Contexts;

namespace Profile.DataAccess.Implementations.BaseRepositories;

public class GenericRepository<T, TKey>(ProfileDbContext _dbContext) : IGenericRepository<T, TKey> where T : BaseModel<TKey>
{
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<T?> FirstOrDefaultAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().WhereNotDeleted().AsNoTracking().FirstOrDefaultAsync(e => e.Id != null && e.Id.Equals(id), cancellationToken);
    }
    
    public IQueryable<T> FindAll()
    {
        return _dbContext.Set<T>().WhereNotDeleted().AsNoTracking();
    }
    
    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return await FindAll().WhereNotDeleted().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
    }
}