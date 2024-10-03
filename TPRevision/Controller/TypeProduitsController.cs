using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tprevision.Models.DataManager;
using tprevision.Models.ModelTemplate;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace tprevision.Controller
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

        public TypeProduitsController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetTypes()
        {
            return await _typeProduitRepository.GetAllAsync();
        }


        // GET: api/TypeProduits/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduit>> GetTypeProduit(int id)
        {
            var typeProduit = await _typeProduitRepository.GetByIdAsync(id);
            if (typeProduit.Value == null)
            {
                return NotFound();
            }
            return typeProduit;
        }

        // PUT: api/TypeProduits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeProduit(int id, TypeProduit typeProduit)
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

            await _typeProduitRepository.PutAsync(typeProduitToUpdate.Value, typeProduit);
            return NoContent();
        }

        // POST: api/TypeProduits
        [HttpPost]
        public async Task<ActionResult<TypeProduit>> PostTypeProduit(TypeProduitSansNavigation typeProduit)
        {
            var nouveauTypeProduit = new TypeProduit
            {
                nomtypeproduit = typeProduit.nomtypeproduit
            };

            await _typeProduitRepository.PostAsync(nouveauTypeProduit);
            return CreatedAtAction("GetTypeProduit", new { id = nouveauTypeProduit.Idtypeproduit }, nouveauTypeProduit);
        }

        // DELETE: api/TypeProduits/5
        [HttpDelete("{id}")]
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
