using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TPRevision.Models.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using tprevision.Models.Repository;
using tprevision.Models.ModelTemplate;

namespace tprevision.Models.DataManager
{
    public class MarqueManager : IDataRepository<Marque>
    {
        private readonly ProduitDbContext? _context;

        public MarqueManager(ProduitDbContext context)
        {
            _context = context;
        } 
        //eee

        public MarqueManager() { }


        public async virtual Task<ActionResult<IEnumerable<Marque>>> GetAllAsync()
        {
            var lesMarques = await _context.Marques.ToListAsync();
            return lesMarques;
        }

        public async virtual Task<ActionResult<Marque>> GetByIdAsync(int id)
        {
            var laMarque = await _context.Marques.FirstOrDefaultAsync(m => m.Idmarque == id);
            return laMarque;
        }

        public async virtual Task<ActionResult<Marque>> GetByStringAsync(string nom)
        {
            var laMarque = await _context.Marques.FirstOrDefaultAsync(m => m.NomMarque.ToUpper() == nom.ToUpper());
            return laMarque;
        }

        public async virtual Task PostAsync(Marque entity)
        {
            await _context.Marques.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task PutAsync(Marque marqueToUpdate, Marque entity)
        {
            _context.Entry(marqueToUpdate).State = EntityState.Modified;
            marqueToUpdate.NomMarque = entity.NomMarque;
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(Marque entity)
        {
            _context.Marques.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
