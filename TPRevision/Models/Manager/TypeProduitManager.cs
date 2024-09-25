using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPRevision.Models.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using tprevision.Models.Repository;

namespace tprevision.Models.DataManager
{
    public class TypeProduitManager : IDataRepository<TypeProduit>
    {
        private readonly ProduitDbContext _context;

        public TypeProduitManager(ProduitDbContext context)
        {
            _context = context;
        }

        public TypeProduitManager() { }

        public virtual ActionResult<IEnumerable<TypeProduit>> GetAll()
        {
            return _context.Types.ToList();
        }

        public virtual ActionResult<TypeProduit> GetById(int id)
        {
            var typeProduit = _context.Types.FirstOrDefault(tp => tp.Idtypeproduit == id);

            return typeProduit;
        }

        public virtual void Post(TypeProduit entity)
        {
            _context.Types.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Put(TypeProduit typeProduitToUpdate, TypeProduit entity)
        {
            _context.Entry(typeProduitToUpdate).State = EntityState.Modified;
            typeProduitToUpdate.nomtypeproduit = entity.nomtypeproduit;
            _context.SaveChanges();
        }

        public virtual void Delete(TypeProduit entity)
        {
            _context.Types.Remove(entity);
            _context.SaveChanges();
        }

        public virtual ActionResult<TypeProduit> GetByString(string str)
        {
            var typeProduit = _context.Types.FirstOrDefault(tp => tp.nomtypeproduit == str);

            return typeProduit;
        }
    }
}
