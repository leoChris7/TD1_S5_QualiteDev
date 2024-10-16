using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GestionProduit_API.Models.Repository;
using GestionProduit_API.Models.EntityFramework;
using AutoMapper;
using GestionProduit_API.Models.DTO;

namespace GestionProduit_API.Models.Manager
{
    public class ProduitManager : IProduitRepository<Produit, ProduitDTO>
    {
        private readonly ProduitDbContext _context;
        private readonly IMapper _mapper;

        public ProduitManager(ProduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Récupérer tous les produits et les mapper vers ProduitDTO
        public async Task<ActionResult<IEnumerable<ProduitDTO>>> GetAllAsync()
        {
            var produits = await _context.Produits.Include(p=>p.TypeProduit).Include(p=>p.Marque).ToListAsync();
            var produitsDto = _mapper.Map<IEnumerable<ProduitDTO>>(produits);
            return new ActionResult<IEnumerable<ProduitDTO>>(produitsDto);
        }

        // Récupérer un produit par ID et le mapper vers ProduitDTO
        public async Task<ActionResult<ProduitDTO>> GetByIdAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return new NotFoundResult();
            }
            var produitDto = _mapper.Map<ProduitDTO>(produit);
            return new ActionResult<ProduitDTO>(produitDto);
        }

        // Récupérer un produit par nom et le mapper vers ProduitDTO
        public async Task<ActionResult<ProduitDTO>> GetByStringAsync(string nom)
        {
            var produit = await _context.Produits.FirstOrDefaultAsync(p => p.NomProduit.ToUpper() == nom.ToUpper());
            if (produit == null)
            {
                return new NotFoundResult();
            }
            var produitDto = _mapper.Map<ProduitDTO>(produit);
            return new ActionResult<ProduitDTO>(produitDto);
        }

        // Ajouter un nouveau produit à partir de ProduitDTO
        public async Task PostAsync(ProduitDTO produitDto)
        {
            var produit = _mapper.Map<Produit>(produitDto);
            await _context.Produits.AddAsync(produit);
            await _context.SaveChangesAsync();
        }

        // Mettre à jour un produit existant à partir de ProduitDTO
        public async Task PutAsync(Produit produitToUpdate, ProduitDTO produitDto)
        {
            var produit = _mapper.Map<Produit>(produitDto);
            _context.Entry(produitToUpdate).CurrentValues.SetValues(produit);
            await _context.SaveChangesAsync();
        }

        // Supprimer un produit existant
        public async Task DeleteAsync(Produit produit)
        {
            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
        }
    }
}
