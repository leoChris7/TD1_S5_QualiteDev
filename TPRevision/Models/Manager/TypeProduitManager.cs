using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPRevision.Models.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public TypeProduitManager()
        {

        }

        public async virtual Task<ActionResult<IEnumerable<TypeProduit>>> GetAllAsync()
        {
            var types = await _context.Types.ToListAsync();
            return new ActionResult<IEnumerable<TypeProduit>>(types);
        }

        public async virtual Task<ActionResult<TypeProduit>> GetByIdAsync(int id)
        {
            var typeProduit = await _context.Types.FirstOrDefaultAsync(tp => tp.Idtypeproduit == id);
            return typeProduit != null ? new ActionResult<TypeProduit>(typeProduit) : new NotFoundResult();
        }

        public async virtual Task PostAsync(TypeProduit entity)
        {
            await _context.Types.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task PutAsync(TypeProduit typeProduitToUpdate, TypeProduit entity)
        {
            _context.Entry(typeProduitToUpdate).State = EntityState.Modified;
            typeProduitToUpdate.nomtypeproduit = entity.nomtypeproduit;
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(TypeProduit entity)
        {
            _context.Types.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task<ActionResult<TypeProduit>> GetByStringAsync(string str)
        {
            var typeProduit = await _context.Types.FirstOrDefaultAsync(tp => tp.nomtypeproduit == str);
            return typeProduit != null ? new ActionResult<TypeProduit>(typeProduit) : new NotFoundResult();
        }
    }
}
