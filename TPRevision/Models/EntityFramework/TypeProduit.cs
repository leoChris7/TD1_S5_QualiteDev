using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GestionProduit_API.Models.EntityFramework
{
    [PrimaryKey("Idtypeproduit")]
    [Table("typeproduit")]
    public partial class TypeProduit
    {
        [Key]
        [Column("idtypeproduit")]
        public int Idtypeproduit { get; set; }

        [Column("nomtypeproduit")]
        public string? Nomtypeproduit { get; set; }

        [InverseProperty(nameof(Produit.IdTypeProduitNavigation))]
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as TypeProduit;
            return other != null && Nomtypeproduit == other.Nomtypeproduit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Idtypeproduit, Nomtypeproduit);
        }
    }
}
