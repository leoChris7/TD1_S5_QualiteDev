using GestionProduit_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface IProduitRepository<Produit, ProduitDto, ProduitDetailDto, ProduitSansNavigation>
    {
        Task<ActionResult<IEnumerable<ProduitDto>>> GetAllAsync();
        Task<ActionResult<ProduitDetailDto>> GetByIdAsync(int id);
        Task<ActionResult<ProduitDetailDto>> GetByStringAsync(string str);
        Task PostAsync(ProduitSansNavigation entityDto);
        Task PutAsync(int id, ProduitSansNavigation entityDto);
        Task DeleteAsync(int id);
    }
}
