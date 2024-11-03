using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionProduit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitManager _produitManager;

        public ProduitsController(ProduitManager manager)
        {
            _produitManager = manager;
        }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProduitDTO>>> GetProduits()
        {
            var produits = await _produitManager.GetAllAsync();
            return Ok(produits.Value);
        }

        /// <summary>
        /// Récupère un produit en fonction de son ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDetailDTO>> GetProduitById(int id)
        {
            var produit = await _produitManager.GetByIdAsync(id);
            return produit.Result is NotFoundResult ? NotFound() : Ok(produit.Value);
        }

        /// <summary>
        /// Récupère un produit en fonction de son nom.
        /// </summary>
        [HttpGet("GetByString/{nom}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDetailDTO>> GetProduitByString(string nom)
        {
            var produit = await _produitManager.GetByStringAsync(nom);
            return produit.Result is NotFoundResult ? NotFound() : Ok(produit.Value);
        }

        /// <summary>
        /// Ajoute un nouveau produit.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProduitDTO>> PostProduit([FromBody] ProduitDTO produitDto)
        {
            if (produitDto == null) return BadRequest("Les données du produit sont manquantes.");

            await _produitManager.PostAsync(produitDto);
            return CreatedAtAction(nameof(GetProduitById), new { id = produitDto.Id }, produitDto);
        }


        /// <summary>
        /// Modifie un produit existant.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduit(int id, ProduitDTO produitDto)
        {
            if (id != produitDto.Id) return BadRequest("L'identifiant du produit ne correspond pas.");
            var produitToUpdate = await _produitManager.GetByIdAsync(id);
            if (produitToUpdate.Result is NotFoundResult) return NotFound();

            await _produitManager.PutAsync(id, produitDto);
            return NoContent();
        }

        /// <summary>
        /// Supprime un produit.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _produitManager.GetByIdAsync(id);
            if (produit.Result is NotFoundResult) return NotFound();

            await _produitManager.DeleteAsync(id);
            return NoContent();
        }
    }
}
