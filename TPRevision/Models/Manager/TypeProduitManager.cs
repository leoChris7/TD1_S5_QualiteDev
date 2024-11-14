using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProduit_API.Models.Repository;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.DTO;
using AutoMapper;

namespace GestionProduit_API.Models.Manager
{
    public class TypeProduitManager : IDataRepository<TypeProduit>
    {
        private readonly ProduitDbContext _context;
        private readonly IMapper _mapper;

        public TypeProduitManager(ProduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TypeProduitManager()
        {

        }

        public async virtual Task<ActionResult<IEnumerable<TypeProduit>>> GetAllAsync()
        {
            var types = await _context.Types
                .Include(p => p.Produits)
                .ToListAsync();

            return types;
        }

        public async virtual Task<ActionResult<IEnumerable<TypeProduitDTO>>> GetAllDTOAsync()
        {
            var lesTypesdeProduit = await _context.Types
                .Include(m => m.Produits)
                .ToListAsync();

            var lesTypesdeProduitDTO = _mapper.Map<List<TypeProduitDTO>>(lesTypesdeProduit);
            return lesTypesdeProduitDTO;
        }

        public async virtual Task<ActionResult<TypeProduit>> GetByIdAsync(int id)
        {
            var typeProduit = await _context.Types
                .Include(m => m.Produits)
                .FirstOrDefaultAsync(tp => tp.Idtypeproduit == id);

            return typeProduit != null ? new ActionResult<TypeProduit>(typeProduit) : new NotFoundResult();
        }

        public async virtual Task<ActionResult<TypeProduit>> GetByStringAsync(string str)
        {
            var typeProduit = await _context.Types.FirstOrDefaultAsync(tp => tp.Nomtypeproduit == str);
            return typeProduit != null ? new ActionResult<TypeProduit>(typeProduit) : new NotFoundResult();
        }

        public async virtual Task PostAsync(TypeProduit entity)
        {
            await _context.Types.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task PutAsync(TypeProduit entityToUpdate, TypeProduit entity)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            entityToUpdate.Nomtypeproduit = entity.Nomtypeproduit;
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(TypeProduit entity)
        {
            _context.Types.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
