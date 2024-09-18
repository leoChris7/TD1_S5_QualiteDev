using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPRevision.Models.EntityFramework;

namespace tprevision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProduitsController : ControllerBase
    {
        private readonly ProduitDbContext _context;

        public TypeProduitsController(ProduitDbContext context)
        {
            _context = context;
        }

        // GET: api/TypeProduits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }

        // GET: api/TypeProduits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeProduit>> GetTypeProduit(int id)
        {
            var typeProduit = await _context.Types.FindAsync(id);

            if (typeProduit == null)
            {
                return NotFound();
            }

            return typeProduit;
        }

        // PUT: api/TypeProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeProduit(int id, TypeProduit typeProduit)
        {
            if (id != typeProduit.Idtypeproduit)
            {
                return BadRequest();
            }

            _context.Entry(typeProduit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeProduitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TypeProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TypeProduit>> PostTypeProduit(TypeProduit typeProduit)
        {
            _context.Types.Add(typeProduit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeProduit", new { id = typeProduit.Idtypeproduit }, typeProduit);
        }

        // DELETE: api/TypeProduits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeProduit(int id)
        {
            var typeProduit = await _context.Types.FindAsync(id);
            if (typeProduit == null)
            {
                return NotFound();
            }

            _context.Types.Remove(typeProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeProduitExists(int id)
        {
            return _context.Types.Any(e => e.Idtypeproduit == id);
        }
    }
}
