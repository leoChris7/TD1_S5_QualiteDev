using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tprevision.Models.DataManager;
using tprevision.Models.Manager.tprevision.Models.DataManager;
using tprevision.Models.ModelTemplate;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace tprevision.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitManager _produitRepository;

        [ActivatorUtilitiesConstructor]
        public ProduitsController(ProduitManager produitRepository)
        {
            _produitRepository = produitRepository;
        }

        public ProduitsController() { }

        // GET: api/Produits
        [HttpGet]
        public ActionResult<IEnumerable<Produit>> GetProduits()
        {
            return _produitRepository.GetAll();
        }

        // GET: api/Produits/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Produit> GetProduit(int id)
        {
            var produit = _produitRepository.GetById(id);
            if (produit == null)
            {
                return NotFound();
            }
            return produit;
        }

        // PUT: api/Produits/5
        [HttpPut("{id}")]
        public IActionResult PutProduit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return BadRequest();
            }

            var produitToUpdate = _produitRepository.GetById(id);
            if (produitToUpdate == null)
            {
                return NotFound();
            }

            _produitRepository.Put(produitToUpdate.Value, produit);
            return NoContent();
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

            _produitRepository.Post(nouveauProduit);
            return CreatedAtAction("GetProduit", new { id = nouveauProduit.IdProduit }, nouveauProduit);
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduit(int id)
        {
            var produit = _produitRepository.GetById(id);
            if (produit == null)
            {
                return NotFound();
            }

            _produitRepository.Delete(produit.Value);
            return NoContent();
        }
    }
}
