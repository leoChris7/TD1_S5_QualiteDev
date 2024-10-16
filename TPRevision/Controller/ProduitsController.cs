using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace GestionProduit_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitManager _produitManager;
        private readonly IMapper _mapper;

        // Constructeur avec injection de dépendances
        [ActivatorUtilitiesConstructor]
        public ProduitsController(ProduitManager manager, IMapper mapper)
        {
            _produitManager = manager;
            _mapper = mapper;
        }

        // Constructeur par défaut
        public ProduitsController() { }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Une liste de produits DTO.</returns>
        /// <response code="200">Retourne la liste des produits.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProduitDTO>>> GetProduits()
        {
            var produits = await _produitManager.GetAllAsync(); // Charge bien les produits avec les Include
            var produitDtos = _mapper.Map<List<ProduitDTO>>(produits); // Map les entités vers DTO
            return Ok(produitDtos);
        }


        /// <summary>
        /// Récupère un produit en fonction de son ID.
        /// </summary>
        /// <param name="id">L'ID du produit.</param>
        /// <returns>Le produit DTO correspondant à l'ID.</returns>
        /// <response code="200">Retourne le produit trouvé.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProduitDTO>> GetProduitById(int id)
        {
            var produit = await _produitManager.GetByIdAsync(id);
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
        /// <returns>Le produit DTO correspondant au nom.</returns>
        /// <response code="200">Retourne le produit trouvé.</response>
        /// <response code="404">Produit introuvable.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpGet("GetByString/{nom}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProduitDTO>> GetProduitByString(string nom)
        {
            var produit = await _produitManager.GetByStringAsync(nom);
            if (produit.Value == null)
            {
                return NotFound();
            }
            return produit;
        }

        /// <summary>
        /// Ajoute un nouveau produit à partir d'un ProduitDTO.
        /// </summary>
        /// <param name="produitDto">Le produit DTO à ajouter.</param>
        /// <returns>Le produit ajouté avec son ID généré.</returns>
        /// <response code="201">Produit créé avec succès.</response>
        /// <response code="500">Erreur interne du serveur.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProduitDTO>> PostProduit(ProduitDTO produitDto)
        {
            await _produitManager.PostAsync(produitDto);
            return CreatedAtAction("GetProduitById", new { id = produitDto.Id }, produitDto);
        }

        /// <summary>
        /// Modifie un produit existant à partir d'un ProduitDTO.
        /// </summary>
        /// <param name="id">L'ID du produit à modifier.</param>
        /// <param name="produitDto">Les nouvelles données du produit.</param>
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
        public async Task<IActionResult> PutProduit(int id, ProduitDTO produitDto)
        {
            if (id != produitDto.Id)
            {
                return BadRequest();
            }

            var produitToUpdate = await _produitManager.GetByIdAsync(id);
            if (produitToUpdate.Value == null)
            {
                return NotFound();
            }

            var produitD = _mapper.Map<Produit>(produitToUpdate.Value);

            await _produitManager.PutAsync(produitD, produitDto);
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
            var produit = await _produitManager.GetByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }

            var produitD = _mapper.Map<Produit>(produit.Value);


            await _produitManager.DeleteAsync(produitD);
            return NoContent();
        }
    }
}
