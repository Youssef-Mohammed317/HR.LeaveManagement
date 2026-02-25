using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HR.LeaveManagement.Persistence.Repositories;

public class GenericRepository<TEntity, TKey>(HrDatabaseContext context) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected DbSet<TEntity> dbSet = context.Set<TEntity>();
    public async Task CreateAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }



    public async Task<IReadOnlyList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        )
    {
        var query = dbSet.AsQueryable();

        if (include != null)
            query = include(query);

        if (filter != null)
            query = query.Where(filter);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey key)
    {
        return await dbSet.FindAsync(key);
    }
    public async Task UpdateAsync(TEntity entity)
    {
        dbSet.Update(entity);
        await context.SaveChangesAsync();
    }
    public async Task<TEntity?> GetFirstAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        )
    {
        var query = dbSet.AsQueryable();

        if (include != null)
            query = include(query);

        return await query.FirstOrDefaultAsync(filter);
    }
    public async Task<bool> ExsistsAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.AnyAsync(filter);
    }

    public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return await query.SumAsync(selector);
    }


}
