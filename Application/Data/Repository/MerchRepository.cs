

using Application.Data.Context;
using Application.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Data.Repository;

public interface IMerchRepository
{
    Task<bool> AddAsync(MerchEntity entity);
    Task<bool> DeleteAsync(Expression<Func<MerchEntity, bool>> expression);
    Task<IEnumerable<MerchEntity>> GetAllAsync(bool sortByDescending = false, Expression<Func<MerchEntity, object>>? sortBy = null, Expression<Func<MerchEntity, bool>>? filterBy = null, params Expression<Func<MerchEntity, object>>[] includes);
    Task<MerchEntity?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(MerchEntity entity);
}

public class MerchRepository : IMerchRepository
{
    private readonly MerchDbContext _context;

    public MerchRepository(MerchDbContext context)
    {
        _context = context;
    }

    public async Task<MerchEntity?> GetByIdAsync(Guid id)
    {
        return await _context.MerchEntitySet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<MerchEntity>> GetAllAsync(bool sortByDescending = false, Expression<Func<MerchEntity, object>>? sortBy = null, Expression<Func<MerchEntity, bool>>? filterBy = null, params Expression<Func<MerchEntity, object>>[] includes)
    {
        IQueryable<MerchEntity> query = _context.MerchEntitySet;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = sortByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        return await query.ToListAsync();
    }


    public virtual async Task<bool> UpdateAsync(MerchEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


    public virtual async Task<bool> AddAsync(MerchEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }


    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<MerchEntity, bool>> expression)
    {
        var entity = await _context.MerchEntitySet.FirstOrDefaultAsync(expression);
        if (entity == null)
            return false;

        try
        {
            _context.MerchEntitySet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

}


