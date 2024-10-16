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
            CreateMap<ProduitSansNavigation, Produit>();
            CreateMap<MarqueSansNavigation, Marque>();
            CreateMap<TypeProduitSansNavigation, Type>();
            CreateMap<ProduitDTO, Produit>();
            CreateMap<ProduitDetailDTO, Produit>();
            CreateMap<MarqueDTO, Marque>();
            CreateMap<TypeProduitDTO, Type>();

            // dans l'autre sens 
            CreateMap<Produit, ProduitSansNavigation>().ReverseMap();
            CreateMap<Produit, ProduitDTO>().ReverseMap();
            CreateMap<Produit, ProduitDetailDTO>().ReverseMap();
            CreateMap<Marque, MarqueDTO>().ReverseMap();
            CreateMap<TypeProduit, TypeProduitDTO>().ReverseMap();
        }
    }
}
