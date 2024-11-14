using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        public Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync();
        public Task<ActionResult<TEntity>> GetByIdAsync(int id);
        public Task<ActionResult<TEntity>> GetByStringAsync(string str);
        public Task PostAsync(TEntity entity);
        public Task PutAsync(TEntity entityToUpdate, TEntity entity);
        public Task DeleteAsync(TEntity entity);
    }

}
 