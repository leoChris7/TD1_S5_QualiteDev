using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TPRevision.Models.EntityFramework
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
        public  int IdTypeProduit { get; set; }

        [ForeignKey("Idmarque")]
        [InverseProperty(nameof(Marque.Produits))]
        public  int IdMarque { get; set; }
        
        [ForeignKey(nameof(IdTypeProduit))]
        [InverseProperty(nameof(TypeProduit.Produits))]
        public virtual TypeProduit IdTypeProduitNavigation { get; set; } = null!;

        [ForeignKey(nameof(IdMarque))]
        [InverseProperty(nameof(Marque.Produits))]
        public virtual Marque IdMarqueNavigation { get; set; } = null!;

        [Column("stockreel")]
        public int StockReel { get => stockReel; set => stockReel = value; }

        [Column("stockmin")]
        public int StockMin { get => stockMin; set => stockMin = value; }

        [Column("stockmax")]
        public int StockMax { get => stockMax; set => stockMax = value; }

    }
}
