namespace DeerFit.Core.Repositories;

public interface IRepository<T>
{
    // CRUD operations - Create, Read, Update, Delete
    Task<List<T>> GetAllAsync();
    Task<T?>      GetByIdAsync(string id);
    Task          CreateAsync(T entity);
    Task          UpdateAsync(string id, T entity);
    Task          DeleteAsync(string id);
}