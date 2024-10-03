using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.ModelTemplate;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;

namespace GestionProduit_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitManager produitManager;

        [ActivatorUtilitiesConstructor]
        public ProduitsController(ProduitManager manager)
        {
            produitManager = manager;
        }

        public ProduitsController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return await produitManager.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduitById(int id)
        {
            var produit = await produitManager.GetByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }
            return produit;
        }

        // Ajout de GetProduitByString pour chercher un produit par nom
        [HttpGet("search/{nom}")]
        public async Task<ActionResult<Produit>> GetProduitByString(string nom)
        {
            var produit = await produitManager.GetByStringAsync(nom);
            if (produit.Value == null)
            {
                return NotFound();
            }
            return produit;
        }

        // POST: api/Produits
        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(ProduitSansNavigation produit)
        {
            var nouveauProduit = new Produit
            {
                NomProduit = produit.NomProduit,
                Description = produit.Description,
                StockMax = produit.StockMax,
                StockMin = produit.StockMin,
                StockReel = produit.StockReel,
                UriPhoto = produit.UriPhoto,
                NomPhoto = produit.NomPhoto,
                IdTypeProduit = produit.IdTypeProduit,
                IdMarque = produit.IdMarque
            };

            await produitManager.PostAsync(nouveauProduit);
            return CreatedAtAction("GetProduit", new { id = nouveauProduit.IdProduit }, nouveauProduit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return BadRequest();
            }

            var produitToUpdate = await produitManager.GetByIdAsync(id);
            if (produitToUpdate.Value == null)
            {
                return NotFound();
            }

            ProduitSansNavigation psN = new ProduitSansNavigation
            {
                NomProduit = produit.NomProduit,
                StockMax = produit.StockMax
            };

            await produitManager.PutAsync(produitToUpdate.Value.IdProduit, psN);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await produitManager.GetByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }
            await produitManager.DeleteAsync(produit.Value);
            return NoContent();
        }
    }
}
