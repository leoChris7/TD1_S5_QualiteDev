using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace tprevision.Models.Manager
{
    public class MarqueManager : IDataRepository<Marque>
    {
        readonly ProduitDbContext _context;
        public MarqueManager()
        {
        }

        public MarqueManager(ProduitDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Marque>> AddAsync(Marque entity)
        {
            _context.Marques.Add(entity);
            await _context.SaveChangesAsync();
            return new OkObjectResult(entity);
        }

        public async Task<ActionResult<Marque>> DeleteAsync(Marque entity)
        {
            var existingEntity = await _context.Marques.FindAsync(entity.Idmarque);
            if (existingEntity == null)
            {
                return new NotFoundResult();
            }

            _context.Marques.Remove(existingEntity);
            await _context.SaveChangesAsync();
            return new OkObjectResult(existingEntity);
        }


        // Obtenir une Marque par son ID
        public async Task<Marque> GetByIdAsync(int id)
        {
            return await _context.Marques.FindAsync(id);
        }

        // Obtenir une Marque par chaîne de caractères
        public async Task<Marque> GetByStringAsync(string str)
        {
            return await _context.Marques.FirstOrDefaultAsync(m => m.NomMarque == str);
        }

        public async Task<ActionResult<Marque>> UpdateAsync(Marque entityToUpdate, Marque entity)
        {
            var existingEntity = await _context.Marques.FindAsync(entityToUpdate.Idmarque);
            if (existingEntity == null)
            {
                return new NotFoundResult();
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return new OkObjectResult(existingEntity);
        }

        async Task<IEnumerable<Marque>> IDataRepository<Marque>.GetAllAsync()
        {
            var entities = await _context.Marques.ToListAsync();
            return entities;
        }
    }
}
