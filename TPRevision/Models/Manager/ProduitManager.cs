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
    public class ProduitManager : IProduitRepository<Produit, ProduitDTO, ProduitDetailDTO, ProduitSansNavigation>
    {
        private readonly ProduitDbContext _context;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public ProduitManager(ProduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ProduitManager()
        {
        }

        public async virtual Task<ActionResult<IEnumerable<ProduitDTO>>> GetAllAsync()
        {
            var produits = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .ToListAsync();
            var produitsDto = _mapper.Map<List<ProduitDTO>>(produits);
            return produitsDto;
        }

        public async virtual Task<ActionResult<ProduitDetailDTO>> GetByIdAsync(int id)
        {
            var produit = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .FirstOrDefaultAsync(p => p.IdProduit == id);

            return produit == null ? new NotFoundResult() : new ActionResult<ProduitDetailDTO>(_mapper.Map<ProduitDetailDTO>(produit));
        }

        public async virtual Task<ActionResult<ProduitDetailDTO>> GetByStringAsync(string nom)
        {
            var produit = await _context.Produits
                                         .Include(p => p.TypeProduit)
                                         .Include(p => p.Marque)
                                         .FirstOrDefaultAsync(p => p.NomProduit.ToUpper() == nom.ToUpper());

            return produit == null ? new NotFoundResult() : new ActionResult<ProduitDetailDTO>(_mapper.Map<ProduitDetailDTO>(produit));
        }

        public async virtual Task PostAsync(ProduitSansNavigation produit)
        {
            
            await _context.Produits.AddAsync(_mapper.Map<Produit>(produit));
            await _context.SaveChangesAsync();
        }

        public async virtual Task PutAsync(int id, ProduitSansNavigation produit)
        {
            var produitToUpdate = await _context.Produits
                .Include(p => p.Marque)
                .Include(p => p.TypeProduit)
                .FirstOrDefaultAsync(p => p.IdProduit == id) ?? throw new KeyNotFoundException("Produit non trouvé");

            // Vérification si chaque propriété est non-null avant la mise à jour
            if (produit.NomProduit != null) produitToUpdate.NomProduit = produit.NomProduit;
            if (produit.NomPhoto != null) produitToUpdate.NomPhoto = produit.NomPhoto;
            if (produit.UriPhoto != null) produitToUpdate.UriPhoto = produit.UriPhoto;
            if (produit.StockMin != 0) produitToUpdate.StockMin = produit.StockMin;
            if (produit.StockMax != 0) produitToUpdate.StockMax = produit.StockMax;
            if (produit.StockReel != 0) produitToUpdate.StockReel = produit.StockReel;
            if (produit.Description != null) produitToUpdate.Description = produit.Description;
            if (produit.IdMarque != 0) produitToUpdate.IdMarque = produit.IdMarque;
            if (produit.IdTypeProduit != 0) produitToUpdate.IdTypeProduit = produit.IdTypeProduit;

            await _context.SaveChangesAsync();
        }


        public async virtual Task DeleteAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) throw new KeyNotFoundException("Produit non trouvé");

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
        }
    }
}
