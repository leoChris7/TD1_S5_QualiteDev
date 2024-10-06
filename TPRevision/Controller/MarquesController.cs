using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.ModelTemplate;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;
using NuGet.Protocol.Core.Types;

namespace GestionProduit_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarquesController : ControllerBase
    {
        private readonly MarqueManager? marqueManager;

        [ActivatorUtilitiesConstructor]
        public MarquesController(MarqueManager _manager)
        {
            marqueManager = _manager ?? throw new ArgumentNullException("Marque manager ne peut pas être null!");
        }

        public MarquesController()
        {
        }

        /// <summary>
        /// Récupère la liste de toutes les marques.
        /// </summary>
        /// <returns>Une liste de Marques.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
        {
            var result = await marqueManager.GetAllAsync();
            return result;
        }

        /// <summary>
        /// Récupère une marque par ID.
        /// </summary>
        /// <param name="id">L'ID de la marque.</param>
        /// <returns>La Marque correspondant à l'ID.</returns>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Marque>> GetMarqueById(int id)
        {
            var marqueById = await marqueManager.GetByIdAsync(id);
            if (marqueById.Value == null)
            {
                return NotFound();
            }

            return marqueById;
        }

        /// <summary>
        /// Récupère une marque par nom.
        /// </summary>
        /// <param name="str">Le nom de la marque.</param>
        /// <returns>La Marque correspondant au nom.</returns>
        [HttpGet("GetByString/{str}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Marque>> GetMarqueByString(string? str)
        {
            var marqueByString = await marqueManager.GetByStringAsync(str);
            if (marqueByString == null)
            {
                return NotFound();
            }
            return marqueByString;
        }

        /// <summary>
        /// Modifie une marque.
        /// </summary>
        /// <param name="id">L'ID de la marque à modifier.</param>
        /// <param name="marque">Les nouvelles données de la marque.</param>
        /// <returns>Aucune réponse si succès.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMarque(int id, MarqueSansNavigation marque)
        {
            if (id != marque.Idmarque)
            {
                return BadRequest();
            }
            var marqueToUpdate = await marqueManager.GetByIdAsync(id);
            if (marqueToUpdate == null)
            {
                return NotFound();
            }

            // Conversion de la marque sans Navigation en un objet marque basique.
            Marque marqueAJour = new Marque
            {
                Idmarque=marque.Idmarque,
                NomMarque=marque.NomMarque
            };

            await marqueManager.PutAsync(marqueToUpdate.Value, marqueAJour);
            return NoContent();
        }

        /// <summary>
        /// Crée une nouvelle marque.
        /// </summary>
        /// <param name="marque">Les informations de la nouvelle marque.</param>
        /// <returns>La Marque créée.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Marque>> PostMarque(MarqueSansNavigation marque)
        {
            Marque NouvelleMarque = new()
            {
                NomMarque = marque.NomMarque
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await marqueManager.PostAsync(NouvelleMarque);
            return CreatedAtAction("GetMarqueById", new { id = NouvelleMarque.Idmarque }, NouvelleMarque);
        }

        /// <summary>
        /// Supprime une marque.
        /// </summary>
        /// <param name="id">L'ID de la marque à supprimer.</param>
        /// <returns>Aucune réponse si succès.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var marque = await marqueManager.GetByIdAsync(id);
            if (marque.Value == null)
            {
                return NotFound(); // Retourne un NotFound si la marque n'existe pas
            }

            await marqueManager.DeleteAsync(marque.Value);

            return NoContent(); // Sinon, supprime et retourne NoContent
        }
    }
}
