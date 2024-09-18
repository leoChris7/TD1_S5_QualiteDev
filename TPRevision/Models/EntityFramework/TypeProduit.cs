using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPRevision.Models.EntityFramework
{
    [PrimaryKey("Idtypeproduit")]
    [Table("typeproduit")]
    public partial class TypeProduit
    {
        [Key]
        [Column("idtypeproduit")]
        public int Idtypeproduit {  get; set; }

        [Column("nomtypeproduit")]
        public string? nomtypeproduit { get; set; }

        [InverseProperty(nameof(Produit.IdTypeProduitNavigation))]
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}
