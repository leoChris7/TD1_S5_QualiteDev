using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.ModelTemplate;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;

namespace GestionProduit_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitManager produitManager;

        // Constructeur utilisant l'injection de dépendances pour fournir un manager de produit
        [ActivatorUtilitiesConstructor]
        public ProduitsController(ProduitManager manager)
        {
            produitManager = manager;
        }

        // Constructeur par défaut
        public ProduitsController() { }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Une liste de produits.</returns>
        /// <response code="200">Retourne la liste des produits.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return await produitManager.GetAllAsync();
        }

        /// <summary>
        /// Récupère un produit en fonction de son ID.
        /// </summary>
        /// <param name="id">L'ID du produit.</param>
        /// <returns>Le produit correspondant à l'ID.</returns>
        /// <response code="200">Retourne le produit trouvé.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Produit>> GetProduitById(int id)
        {
            var produit = await produitManager.GetByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }
            return produit;
        }

        /// <summary>
        /// Récupère un produit en fonction de son nom.
        /// </summary>
        /// <param name="nom">Le nom du produit.</param>
        /// <returns>Le produit correspondant au nom.</returns>
        /// <response code="200">Retourne le produit trouvé.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet("GetByString/{nom}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Produit>> GetProduitByString(string nom)
        {
            var produit = await produitManager.GetByStringAsync(nom);
            if (produit.Value == null)
            {
                return NotFound();
            }
            return produit;
        }

        /// <summary>
        /// Ajoute un nouveau produit.
        /// </summary>
        /// <param name="produit">Le produit à ajouter.</param>
        /// <returns>Le produit ajouté avec son ID généré.</returns>
        /// <response code="201">Produit créé avec succès.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Modifie un produit existant.
        /// </summary>
        /// <param name="id">L'ID du produit à modifier.</param>
        /// <param name="produit">Les nouvelles données du produit.</param>
        /// <returns>Une réponse HTTP indiquant le succès ou l'échec de la modification.</returns>
        /// <response code="204">Produit modifié avec succès.</response>
        /// <response code="400">Mauvaise requête, l'ID ne correspond pas.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutProduit(int id, ProduitSansNavigation produit)
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

            if (id != produit.IdProduit)
            {
                return BadRequest();
            }

            var produitToUpdate = await produitManager.GetByIdAsync(id);
            if (produitToUpdate.Value == null)
            {
                return NotFound();
            }

            await produitManager.PutAsync(produitToUpdate.Value, nouveauProduit);
            return NoContent();
        }

        /// <summary>
        /// Supprime un produit en fonction de son ID.
        /// </summary>
        /// <param name="id">L'ID du produit à supprimer.</param>
        /// <returns>Une réponse HTTP indiquant le succès ou l'échec de la suppression.</returns>
        /// <response code="204">Produit supprimé avec succès.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
