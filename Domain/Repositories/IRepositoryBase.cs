using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity?> GetByIdAsync(int id);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
