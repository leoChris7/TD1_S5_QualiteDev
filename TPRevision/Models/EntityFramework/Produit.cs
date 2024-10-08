﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduit_API.Models.EntityFramework
{
    [PrimaryKey("IdProduit")]
    [Table("produit")]
    public partial class Produit
    {
        private int idProduit;
        private string? nomProduit;
        private string? description;
        private string? nomPhoto;
        private string? uriPhoto;
        private int idTypeProduit;
        private int idMarque;
        private int stockReel;
        private int stockMin;
        private int stockMax;

        [Key]
        [Column("idproduit")]
        public int IdProduit { get => idProduit; set => idProduit = value; }

        [Column("nomproduit")]
        public string? NomProduit { get => nomProduit; set => nomProduit = value; }

        [Column("description")]
        public string? Description { get => description; set => description = value; }

        [Column("nomphoto")]
        public string? NomPhoto { get => nomPhoto; set => nomPhoto = value; }

        [Column("uriphoto")]
        public string? UriPhoto { get => uriPhoto; set => uriPhoto = value; }

        [ForeignKey("Idtypeproduit")]
        [InverseProperty(nameof(TypeProduit.Produits))]
        public int? IdTypeProduit { get; set; }

        [ForeignKey("Idmarque")]
        [InverseProperty(nameof(Marque.Produits))]
        public int? IdMarque { get; set; }

        //[ForeignKey(nameof(IdTypeProduit))]
        //[InverseProperty(nameof(TypeProduit.Produits))]
        public virtual TypeProduit TypeProduit { get; set; } = null!;

        //[ForeignKey(nameof(IdMarque))]
        //[InverseProperty(nameof(Marque.Produits))]
        public virtual Marque Marque { get; set; } = null!;

        [Range(0, int.MaxValue, MinimumIsExclusive = false, MaximumIsExclusive = false)]
        [Column("stockreel")]
        public int StockReel { get => stockReel; set => stockReel = value; }

        [Range(0, int.MaxValue, MinimumIsExclusive = false, MaximumIsExclusive = false)]
        [Column("stockmin")]
        public int StockMin { get => stockMin; set => stockMin = value; }

        [Range(1, int.MaxValue, MinimumIsExclusive = false, MaximumIsExclusive = false)]
        [Column("stockmax")]
        public int StockMax { 
            get => stockMax; 
            set => stockMax = value; 
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Produit;
            return other != null && IdProduit == other.IdProduit && NomProduit == other.NomProduit &&
                    IdTypeProduit == other.IdTypeProduit && IdMarque == other.IdMarque &&
                    StockReel == other.StockReel && StockMin == other.StockMin && StockMax == other.StockMax;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdProduit, NomProduit, IdTypeProduit, IdMarque, StockReel, StockMin, StockMax);
        }
    }
}
