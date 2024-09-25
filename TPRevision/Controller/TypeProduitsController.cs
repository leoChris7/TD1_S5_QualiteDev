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

        public TypeProduitsController(TypeProduitManager typeProduitRepository)
        {
            _typeProduitRepository = typeProduitRepository;
        }

        public TypeProduitsController() { }

        // GET: api/TypeProduits
        [HttpGet]
        public ActionResult<IEnumerable<TypeProduit>> GetTypes()
        {
            return _typeProduitRepository.GetAll();
        }

        // GET: api/TypeProduits/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TypeProduit> GetTypeProduit(int id)
        {
            var typeProduit = _typeProduitRepository.GetById(id);
            if (typeProduit == null)
            {
                return NotFound();
            }
            return typeProduit;
        }

        // PUT: api/TypeProduits/5
        [HttpPut("{id}")]
        public IActionResult PutTypeProduit(int id, TypeProduit typeProduit)
        {
            if (id != typeProduit.Idtypeproduit)
            {
                return BadRequest();
            }

            var typeProduitToUpdate = _typeProduitRepository.GetById(id);
            if (typeProduitToUpdate == null)
            {
                return NotFound();
            }

            _typeProduitRepository.Put(typeProduitToUpdate.Value, typeProduit);
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

            _typeProduitRepository.Post(nouveauTypeProduit);
            return CreatedAtAction("GetTypeProduit", new { id = nouveauTypeProduit.Idtypeproduit }, nouveauTypeProduit);
        }

        // DELETE: api/TypeProduits/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTypeProduit(int id)
        {
            var typeProduit = _typeProduitRepository.GetById(id);
            if (typeProduit == null)
            {
                return NotFound();
            }

            _typeProduitRepository.Delete(typeProduit.Value);
            return NoContent();
        }
    }
}
