using Microsoft.AspNetCore.Mvc;

namespace tprevision.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByStringAsync(string str);
        Task<ActionResult<TEntity>> AddAsync(TEntity entity);
        Task<ActionResult<TEntity>> UpdateAsync(TEntity entityToUpdate, TEntity entity);
        Task<ActionResult<TEntity>> DeleteAsync(TEntity entity);
    }
}
