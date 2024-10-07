using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.Repository;
using GestionProduit_API.Models.EntityFramework;

namespace GestionProduit_API.Models.Manager
{
    public class ProduitManager : IDataRepository<Produit>
    {
        private readonly ProduitDbContext _context;

        public ProduitManager(ProduitDbContext context)
        {
            _context = context;
        }

        public ProduitManager()
        {

        }

        public async virtual Task<ActionResult<IEnumerable<Produit>>> GetAllAsync()
        {
            return await _context.Produits.ToListAsync();
        }

        public async virtual Task<ActionResult<Produit>> GetByIdAsync(int id)
        {
            var produit = await _context.Produits.FirstOrDefaultAsync(p => p.IdProduit == id);
            return produit;
        }

        // Ajout de GetByStringAsync pour chercher un produit par son nom
        public async virtual Task<ActionResult<Produit>> GetByStringAsync(string nom)
        {
            var produit = await _context.Produits.FirstOrDefaultAsync(p => p.NomProduit.ToUpper() == nom.ToUpper());
            return produit;
        }

        public async virtual Task PostAsync(Produit entity)
        {            
            await _context.Produits.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task PutAsync(Produit produitToUpdate, Produit entity)
        {
            _context.Entry(produitToUpdate).State = EntityState.Modified;

            produitToUpdate.NomProduit = entity.NomProduit;
            produitToUpdate.IdTypeProduit = entity.IdTypeProduit;
            produitToUpdate.IdMarque = entity.IdMarque;
            produitToUpdate.StockMax = entity.StockMax;
            produitToUpdate.StockMin = entity.StockMin;
            produitToUpdate.StockReel = entity.StockReel;
            produitToUpdate.UriPhoto = entity.UriPhoto;
            produitToUpdate.NomPhoto = entity.NomPhoto;
            produitToUpdate.Description = entity.Description;

            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(Produit entity)
        {
            _context.Produits.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
