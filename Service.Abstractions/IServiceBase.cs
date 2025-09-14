namespace Service.Abstractions;

public interface IServiceBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task UpdateAsync(int id, T entity);

    Task DeleteAsync(int id);
}