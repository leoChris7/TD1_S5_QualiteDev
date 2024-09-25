namespace tprevision.Models.Manager
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TPRevision.Models.EntityFramework;
    using System.Collections.Generic;
    using System.Linq;
    using global::tprevision.Models.Repository;
    using Microsoft.AspNetCore.Http.HttpResults;

    namespace tprevision.Models.DataManager
    {
        public class ProduitManager : IDataRepository<Produit>
        {
            private readonly ProduitDbContext _context;

            public ProduitManager(ProduitDbContext context)
            {
                _context = context;
            }

            public ProduitManager() { }

            public virtual ActionResult<IEnumerable<Produit>> GetAll()
            {
                return _context.Produits.ToList();
            }

            public virtual ActionResult<Produit> GetById(int id)
            {
                var produit = _context.Produits.FirstOrDefault(p => p.IdProduit == id);

                return produit;
            }

            public virtual void Post(Produit entity)
            {
                _context.Produits.Add(entity);
                _context.SaveChanges();
            }

            public virtual void Put(Produit produitToUpdate, Produit entity)
            {
                _context.Entry(produitToUpdate).State = EntityState.Modified;
                produitToUpdate.NomProduit = entity.NomProduit;
                // Ajoutez d'autres propriétés à mettre à jour si nécessaire
                _context.SaveChanges();
            }

            public virtual void Delete(Produit entity)
            {
                _context.Produits.Remove(entity);
                _context.SaveChanges();
            }

            public virtual ActionResult<Produit> GetByString(string str)
            {
                var produit = _context.Produits.FirstOrDefault(tp => tp.NomProduit == str);

                return produit;
            }
        }
    }

}
