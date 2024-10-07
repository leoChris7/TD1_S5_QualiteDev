using AutoMapper;
using GestionProduit_API.Models.DTO;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.ModelTemplate;

namespace GestionProduit_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Produit, ProduitDTO>();
            CreateMap<Produit, ProduitSansNavigation>();
        }
    }
}
