using AutoMapper;
using GestionProduit_API.Models.DTO;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.ModelTemplate;

namespace GestionProduit_API
{
    public class AutoMapperProfile : Profile
    {
        /**
         * Méthode initilisant le mapper automatique des classe Produit, Marque et TypeProduit
         */

        public AutoMapperProfile()
        {
            // Mapping Produit <-> ProduitDTO
            CreateMap<Produit, ProduitDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeProduit.Nomtypeproduit))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.Marque.NomMarque))
                .ReverseMap();

            // Mapping Produit <-> ProduitDetailDTO
            CreateMap<Produit, ProduitDetailDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeProduit.Nomtypeproduit))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.Marque.NomMarque))
                .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => src.StockReel < src.StockMin))
                .ReverseMap();

            // Mapping pour Marque et TypeProduit
            CreateMap<Marque, MarqueDTO>().ReverseMap();
            CreateMap<TypeProduit, TypeProduitDTO>().ReverseMap();
        }
        


    }
}
