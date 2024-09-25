using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TPRevision.Models.EntityFramework;

namespace tprevision.Models.ModelTemplate
{
    public class TypeProduitSansNavigation
    {

        public int Idtypeproduit { get; set; }

        public string? nomtypeproduit { get; set; }
    }
}
