using GestionProduit_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface IProduitRepository<TEntity, TDto>
    {
        Task<ActionResult<IEnumerable<TDto>>> GetAllAsync();
        Task<ActionResult<TDto>> GetByIdAsync(int id);
        Task<ActionResult<TDto>> GetByStringAsync(string str);
        Task PostAsync(TDto entityDto);
        Task PutAsync(TEntity entityToUpdate, TDto entityDto);
        Task DeleteAsync(TEntity entity);
    }
}
