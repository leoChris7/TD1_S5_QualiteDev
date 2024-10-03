using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduit_API.Models.EntityFramework
{
    [PrimaryKey("Idmarque")]
    [Table("marque")]
    public partial class Marque
    {
        [Key]
        [Column("idmarque")]
        public int Idmarque { get; set; }

        [Column("nommarque")]
        public string? NomMarque { get; set; }

        [InverseProperty(nameof(Produit.IdMarqueNavigation))]
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}
