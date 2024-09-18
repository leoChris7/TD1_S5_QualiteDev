using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace tprevision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly IDataRepository<Produit> dataRepository;

        public ProduitsController(IDataRepository<Produit> newdataRepository)
        {
            dataRepository = newdataRepository;
        }

        // GET: api/Produits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return await dataRepository.GetAllAsync();
        }

        // GET: api/Produits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);

            if (result is NotFoundResult)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Produits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return BadRequest(); // Mauvais ID passé
            }

            var existingProduit = await dataRepository.GetByIdAsync(id);
            if (existingProduit == null)
            {
                return NotFound(); // Le produit à mettre à jour n'existe pas
            }

            // Mettre à jour les valeurs du produit existant avec les nouvelles valeurs
            _context.Entry(existingProduit).CurrentValues.SetValues(produit);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(id))
                {
                    return NotFound(); // Le produit a été supprimé par un autre processus
                }
                else
                {
                    throw; // Relancer l'exception pour la gestion externe
                }
            }

            return NoContent(); // Mise à jour réussie, aucun contenu à retourner
        }

        // POST: api/Produits
        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            var result = await dataRepository.AddAsync(produit);
            if (result is BadRequestResult)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProduit), new { id = produit.IdProduit }, produit);
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await dataRepository.GetByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            var result = await dataRepository.DeleteAsync(produit);
            if (result is NotFoundResult)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool ProduitExists(int id)
        {

            return dataRepository.GetAllAsync.Any(e => e.IdProduit == id);
        }
    }
}
