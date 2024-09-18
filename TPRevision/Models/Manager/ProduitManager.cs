using Microsoft.AspNetCore.Mvc;
using TPRevision.Models.EntityFramework;
using tprevision.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace tprevision.Models.Manager
{
    public class ProduitManager : IDataRepository<Produit>
    {
        readonly ProduitDbContext _context;

        public ProduitManager() { }

        public ProduitManager(ProduitDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Produit>> AddAsync(Produit entity)
        {
            _context.Produits.Add(entity);
            await _context.SaveChangesAsync();
            return new OkObjectResult(entity);
        }

        public async Task<ActionResult<Produit>> DeleteAsync(Produit entity)
        {
            var existingEntity = await _context.Produits.FindAsync(entity.IdProduit);
            if (existingEntity == null)
            {
                return new NotFoundResult();
            }

            _context.Produits.Remove(existingEntity);
            await _context.SaveChangesAsync();
            return new OkObjectResult(existingEntity);
        }

        // Obtenir un Produit par son ID
        public async Task<Produit> GetByIdAsync(int id)
        {
            return await _context.Produits.FindAsync(id);
        }

        // Obtenir un Produit par chaîne de caractères (ex. nom produit)
        public async Task<Produit> GetByStringAsync(string str)
        {
            return await _context.Produits.FirstOrDefaultAsync(p => p.NomProduit == str);
        }

        public async Task<ActionResult<Produit>> UpdateAsync(Produit entityToUpdate, Produit entity)
        {
            var existingEntity = await _context.Produits.FindAsync(entityToUpdate.IdProduit);
            if (existingEntity == null)
            {
                return new NotFoundResult();
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return new OkObjectResult(existingEntity);
        }

        async Task<IEnumerable<Produit>> IDataRepository<Produit>.GetAllAsync()
        {
            var entities = await _context.Produits.ToListAsync();
            return entities;
        }
    }
}
