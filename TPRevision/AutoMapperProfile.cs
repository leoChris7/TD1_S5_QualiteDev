using AutoMapper;
using GestionProduit_API.Models.DTO;
using GestionProduit_API.Models.EntityFramework;

namespace GestionProduit_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping de Produit <-> ProduitDTO avec prise en charge de l'ID et des propriétés complexes
            CreateMap<Produit, ProduitDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduit))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeProduit.Nomtypeproduit))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.Marque.NomMarque))
                .ReverseMap()
                .ForMember(dest => dest.IdProduit, opt => opt.MapFrom(src => src.Id)) // Corrige l'ID
                .ForMember(dest => dest.NomProduit, opt => opt.MapFrom(src => src.Nom))
                .ForMember(dest => dest.TypeProduit, opt => opt.Ignore()) // On ignore les objets complexes
                .ForMember(dest => dest.Marque, opt => opt.Ignore());

            CreateMap<Produit, ProduitSansNavigation>().ReverseMap();

            CreateMap<Produit, ProduitDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduit))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeProduit.Nomtypeproduit))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.Marque.NomMarque))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockReel))

                .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => src.StockReel < src.StockMin))
                .ReverseMap()
                .ForMember(dest => dest.IdProduit, opt => opt.MapFrom(src => src.Id)) // Corrige l'ID
                .ForMember(dest => dest.TypeProduit, opt => opt.Ignore())
                .ForMember(dest => dest.Marque, opt => opt.Ignore());

            // Mapping Marque et TypeProduit
            CreateMap<Marque, MarqueDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Idmarque))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomMarque))
                .ForPath(dest => dest.NbProduits, opt => opt.MapFrom(src => src.Produits.Count))
                .ReverseMap()
                .ForMember(dest => dest.Idmarque, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NomMarque, opt => opt.MapFrom(src => src.Nom));

            CreateMap<Marque, MarqueSansNavigation>().ReverseMap();

            CreateMap<TypeProduit, TypeProduitDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Idtypeproduit))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nomtypeproduit))
                .ForPath(dest => dest.NbProduits, opt => opt.MapFrom(src => src.Produits.Count))
                .ReverseMap();
        }
    }
}
