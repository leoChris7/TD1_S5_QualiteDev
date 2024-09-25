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

        public MarqueManager() { }


        public virtual ActionResult<IEnumerable<Marque>> GetAll()
        {
            return _context.Marques.ToList();
        }

        public virtual ActionResult<Marque> GetById(int id)
        {
            return _context.Marques.FirstOrDefault(m => m.Idmarque == id);
        }

        public virtual ActionResult<Marque> GetByString(string nom)
        {
            return _context.Marques.FirstOrDefault(m => m.NomMarque.ToUpper() == nom.ToUpper());
        }

        public virtual void Post(Marque entity)
        {
            _context.Marques.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Put(Marque marqueToUpdate, Marque entity)
        {
            _context.Entry(marqueToUpdate).State = EntityState.Modified;
            marqueToUpdate.NomMarque = entity.NomMarque;
            _context.SaveChanges();
        }

        public virtual void Delete(Marque entity)
        {
            _context.Marques.Remove(entity);
            _context.SaveChanges();
        }
    }
}
