using GestionProduit_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface IProduitRepository<Produit, ProduitDto, ProduitDetailDto>
    {
        Task<ActionResult<IEnumerable<ProduitDto>>> GetAllAsync();
        Task<ActionResult<ProduitDetailDto>> GetByIdAsync(int id);
        Task<ActionResult<ProduitDetailDto>> GetByStringAsync(string str);
        Task PostAsync(ProduitDto entityDto);
        Task PutAsync(int id, ProduitDto entityDto);
        Task DeleteAsync(int id);
    }
}
