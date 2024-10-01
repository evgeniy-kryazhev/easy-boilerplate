namespace EasyBoilerplate.Shared;

public interface IRepository<TEntity> 
    where TEntity : Entity
{
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity?> GetAsync(Guid id);
    Task<List<TEntity>> GetListAsync();
    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(IEnumerable<TEntity> entity);
    Task Delete(TEntity entity);
}