using Microsoft.EntityFrameworkCore;
using TPRevision.Models.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tprevision.Models.Repository;
using Microsoft.AspNetCore.Mvc;

public class EfDataRepository<T> : IDataRepository<T> where T : class
{
    private readonly ProduitDbContext _context;

    public EfDataRepository(ProduitDbContext context)
    {
        _context = context;
    }

    // Implémentation correcte de GetAllAsync
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    // Implémentation correcte de GetByIdAsync
    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    // Implémentation de AddAsync
    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Implémentation de UpdateAsync
    public async Task<T> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(id);
        if (existingEntity == null)
            return null;

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existingEntity;
    }

    // Implémentation de DeleteAsync
    public async Task<T> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
            return null;

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Implémentation de GetByStringAsync
    public async Task<T> GetByStringAsync(string str)
    {
        var entityType = typeof(T);
        var propertyInfo = entityType.GetProperty(str);

        if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
        {
            throw new ArgumentException("Property not found or not of type string.");
        }

        var query = _context.Set<T>().AsQueryable();
        query = query.Where(x => (string)propertyInfo.GetValue(x) == str);

        return await query.FirstOrDefaultAsync();
    }

    async Task<ActionResult> IDataRepository<T>.AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    Task<ActionResult> IDataRepository<T>.UpdateAsync(T entityToUpdate, T entity)
    {
        throw new NotImplementedException();
    }

    Task<ActionResult> IDataRepository<T>.DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
