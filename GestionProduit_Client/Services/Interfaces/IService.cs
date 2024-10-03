using GestionProduit_Client.Models;

namespace GestionProduit_Client.Services.Interfaces
{
    public interface IService
    {
        Task<List<Produit>> GetProduitsAsync(string nomControleur);
    }
}
