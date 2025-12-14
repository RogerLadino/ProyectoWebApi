using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(RepositoryDbContext repositoryDbContext)
    {
        _dbSet = repositoryDbContext.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}