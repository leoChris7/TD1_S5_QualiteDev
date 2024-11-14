using GestionProduit_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface ICategoryRepository<Category, CategoryDto>
    {
        Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<ActionResult<CategoryDto>> GetByIdAsync(int id);
        Task<ActionResult<Category>> GetByStringAsync(string str);
        Task PostAsync(Category entityDto);
        Task PutAsync(int id, Category entityDto);
        Task DeleteAsync(int id);
    }
}
