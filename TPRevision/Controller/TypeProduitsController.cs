using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.ModelTemplate;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;

namespace GestionProduit_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProduitsController : ControllerBase
    {
        private readonly TypeProduitManager _typeProduitRepository;

        [ActivatorUtilitiesConstructor]
        public TypeProduitsController(TypeProduitManager typeProduitRepository)
        {
            _typeProduitRepository = typeProduitRepository;
        }

        public TypeProduitsController() { }

        /// <summary>
        /// Récupère la liste de tous les types de produits.
        /// </summary>
        /// <returns>Une liste de TypeProduit.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetTypes()
        {
            return await _typeProduitRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère un type de produit par ID.
        /// </summary>
        /// <param name="id">L'ID du type de produit.</param>
        /// <returns>Le TypeProduit correspondant à l'ID.</returns>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduit>> GetTypeProduitById(int id)
        {
            var typeProduit = await _typeProduitRepository.GetByIdAsync(id);
            if (typeProduit.Value == null)
            {
                return NotFound();
            }
            return typeProduit;
        }

        /// <summary>
        /// Récupère un type de produit par son nom.
        /// </summary>
        /// <param name="nomType">Le nom du type de produit.</param>
        /// <returns>Le TypeProduit correspondant au nom.</returns>
        [HttpGet("GetByString/{nomType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduit>> GetTypeProduitByString(string? nomType)
        {
            var typeProduit = await _typeProduitRepository.GetByStringAsync(nomType);
            if (typeProduit.Value == null)
            {
                return NotFound();
            }
            return typeProduit;
        }

        /// <summary>
        /// Modifie un type de produit.
        /// </summary>
        /// <param name="id">L'ID du type de produit à modifier.</param>
        /// <param name="typeProduit">Les nouvelles données du type de produit.</param>
        /// <returns>Aucune réponse si succès.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTypeProduit(int id, TypeProduitSansNavigation typeProduit)
        {
            if (id != typeProduit.Idtypeproduit)
            {
                return BadRequest();
            }

            var typeProduitToUpdate = await _typeProduitRepository.GetByIdAsync(id);
            if (typeProduitToUpdate.Value == null)
            {
                return NotFound();
            }

            TypeProduit nouveauProduit = new TypeProduit
            {
                Idtypeproduit = typeProduit.Idtypeproduit,
                Nomtypeproduit = typeProduit.nomtypeproduit
            };

            await _typeProduitRepository.PutAsync(typeProduitToUpdate.Value, nouveauProduit);
            return NoContent();
        }

        /// <summary>
        /// Crée un nouveau type de produit.
        /// </summary>
        /// <param name="typeProduit">Les informations du nouveau type de produit.</param>
        /// <returns>Le TypeProduit créé.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeProduit>> PostTypeProduit(TypeProduitSansNavigation typeProduit)
        {
            var nouveauTypeProduit = new TypeProduit
            {
                Nomtypeproduit = typeProduit.nomtypeproduit
            };

            await _typeProduitRepository.PostAsync(nouveauTypeProduit);
            return CreatedAtAction("GetTypeProduitById", new { id = nouveauTypeProduit.Idtypeproduit }, nouveauTypeProduit);
        }

        /// <summary>
        /// Supprime un type de produit.
        /// </summary>
        /// <param name="id">L'ID du type de produit à supprimer.</param>
        /// <returns>Aucune réponse si succès.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTypeProduit(int id)
        {
            var typeProduit = await _typeProduitRepository.GetByIdAsync(id);
            if (typeProduit.Value == null)
            {
                return NotFound();
            }

            await _typeProduitRepository.DeleteAsync(typeProduit.Value);
            return NoContent();
        }
    }
}
