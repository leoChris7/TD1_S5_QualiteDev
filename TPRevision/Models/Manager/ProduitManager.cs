using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProduit_API.Models.DTO;
using GestionProduit_API.Models.Repository;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionProduit_API.Models.EntityFramework;

namespace GestionProduit_API.Models.Manager
{
    /// <summary>
    /// Manager pour gérer les opérations liées aux produits, en utilisant les DTOs pour la manipulation des données.
    /// </summary>
    public class ProduitManager : IProduitRepository<Produit, ProduitDTO, ProduitDetailDTO>
    {
        private readonly ProduitDbContext _context;
        private readonly IMapper _mapper;

        public ProduitManager(ProduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<ProduitDTO>>> GetAllAsync()
        {
            var produits = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .ToListAsync();
            var produitsDto = _mapper.Map<List<ProduitDTO>>(produits);
            return produitsDto;
        }

        public async Task<ActionResult<ProduitDetailDTO>> GetByIdAsync(int id)
        {
            var produit = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .FirstOrDefaultAsync(p => p.IdProduit == id);

            return produit == null ? new NotFoundResult() : new ActionResult<ProduitDetailDTO>(_mapper.Map<ProduitDetailDTO>(produit));
        }

        public async Task<ActionResult<ProduitDetailDTO>> GetByStringAsync(string nom)
        {
            var produit = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .FirstOrDefaultAsync(p => p.NomProduit.ToUpper() == nom.ToUpper());

            return produit == null ? new NotFoundResult() : new ActionResult<ProduitDetailDTO>(_mapper.Map<ProduitDetailDTO>(produit));
        }

public async Task PostAsync(ProduitDTO produitDto)
{
    var produit = _mapper.Map<Produit>(produitDto);

    // Recherche de la Marque et du TypeProduit correspondants
    produit.Marque = await _context.Marques.FirstOrDefaultAsync(m => m.NomMarque == produitDto.Marque);
    produit.TypeProduit = await _context.Types.FirstOrDefaultAsync(tp => tp.Nomtypeproduit == produitDto.Type);

    if (produit.Marque == null || produit.TypeProduit == null) { 
        throw new InvalidOperationException("Marque ou Type de produit introuvable");
                }

    await _context.Produits.AddAsync(produit);
    await _context.SaveChangesAsync();
}

public async Task PutAsync(int id, ProduitDTO produitDto)
{
    var produitToUpdate = await _context.Produits.Include(p => p.Marque).Include(p => p.TypeProduit).FirstOrDefaultAsync(p => p.IdProduit == id);

    if (produitToUpdate == null)
        throw new KeyNotFoundException("Produit non trouvé");

    // Mise à jour des propriétés via AutoMapper
    _mapper.Map(produitDto, produitToUpdate);

    // Mise à jour de la Marque et du TypeProduit si nécessaire
    produitToUpdate.Marque = await _context.Marques.FirstOrDefaultAsync(m => m.NomMarque == produitDto.Marque);
    produitToUpdate.TypeProduit = await _context.Types.FirstOrDefaultAsync(tp => tp.Nomtypeproduit == produitDto.Type);

    if (produitToUpdate.Marque == null || produitToUpdate.TypeProduit == null)
        throw new InvalidOperationException("Marque ou Type de produit introuvable");

    await _context.SaveChangesAsync();
}

        public async Task DeleteAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) throw new KeyNotFoundException("Produit non trouvé");

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
        }
    }
}
