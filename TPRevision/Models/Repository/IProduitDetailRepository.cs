using GestionProduit_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GestionProduit_API.Models.Repository
{
    public interface IProduitDetailRepository<ProduitDetailDTO>
    {
        Task<ActionResult<ProduitDetailDTO>> GetProduitDetailByIdAsync(int id);
        Task<ActionResult<IEnumerable<ProduitDetailDTO>>> GetAllProduitDetailsAsync();
    }
}
