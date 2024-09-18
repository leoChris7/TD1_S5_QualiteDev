namespace tprevision.Models.Manager
{
    using global::tprevision.Models.Repository;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TPRevision.Models.EntityFramework;

    namespace tprevision.Models.Manager
    {
        public class TypeProduitManager : IDataRepository<TypeProduit>
        {
            readonly ProduitDbContext _context;

            public TypeProduitManager() { }

            public TypeProduitManager(ProduitDbContext context)
            {
                _context = context;
            }

            public async Task<ActionResult<TypeProduit>> AddAsync(TypeProduit entity)
            {
                _context.Types.Add(entity);
                await _context.SaveChangesAsync();
                return new OkObjectResult(entity);
            }

            public async Task<ActionResult<TypeProduit>> DeleteAsync(TypeProduit entity)
            {
                var existingEntity = await _context.Types.FindAsync(entity.Idtypeproduit);
                if (existingEntity == null)
                {
                    return new NotFoundResult();
                }

                _context.Types.Remove(existingEntity);
                await _context.SaveChangesAsync();
                return new OkObjectResult(existingEntity);
            }

            // Obtenir un TypeProduit par son ID
            public async Task<TypeProduit> GetByIdAsync(int id)
            {
                return await _context.Types.FindAsync(id);
            }

            // Obtenir un TypeProduit par chaîne de caractères (ex. nom type)
            public async Task<TypeProduit> GetByStringAsync(string str)
            {
                return await _context.Types.FirstOrDefaultAsync(tp => tp.nomtypeproduit == str);
            }

            public async Task<ActionResult<TypeProduit>> UpdateAsync(TypeProduit entityToUpdate, TypeProduit entity)
            {
                var existingEntity = await _context.Types.FindAsync(entityToUpdate.Idtypeproduit);
                if (existingEntity == null)
                {
                    return new NotFoundResult();
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                return new OkObjectResult(existingEntity);
            }

            async Task<IEnumerable<TypeProduit>> IDataRepository<TypeProduit>.GetAllAsync()
            {
                var entities = await _context.Types.ToListAsync();
                return entities;
            }
        }
    }

}
