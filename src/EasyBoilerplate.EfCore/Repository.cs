using EasyBoilerplate.Shared;
using Microsoft.EntityFrameworkCore;

namespace EasyBoilerplate.EfCore;

public class Repository<TEntity>(ApplicationDbContext context) : 
    IRepository<TEntity> where TEntity: Entity
{
    protected DbSet<TEntity> GetDbSet()
    {
        return context.Set<TEntity>();
    }
    
    protected IQueryable<TEntity> GetQuery()
    {
        return GetDbSet().AsQueryable();
    }
    
    protected async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var entry = await GetDbSet().AddAsync(entity);
        await SaveAsync();
        return entry.Entity;
    }

    public async Task<TEntity?> GetAsync(Guid id)
    {
        return await GetQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> GetListAsync()
    {
        return await GetQuery().ToListAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await UpdateAsync([entity]);
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entity)
    {
        GetDbSet().UpdateRange(entity);
        await SaveAsync();
    }

    public async Task Delete(TEntity entity)
    {
        GetDbSet().Remove(entity);
        await SaveAsync();
    }
}