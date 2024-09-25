using Microsoft.AspNetCore.Mvc;

namespace tprevision.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        ActionResult<IEnumerable<TEntity>> GetAll();
        ActionResult<TEntity> GetById(int id);
        ActionResult<TEntity> GetByString(string str);
        void Post(TEntity entity);
        void Put(TEntity entityToUpdate, TEntity entity);
        void Delete(TEntity entity);
    }
}
