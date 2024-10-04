using GestionProduit_Client.Models;
using GestionProduit_Client.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionProduit_Client.Services
{
    public class WSService : IService
    {
        private readonly HttpClient _client;


        public WSService(string uri = "http://localhost:5012/api/")
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(uriString: uri)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /**
         *  <summary> Permet de récupérer les produits de façon asynchrone en requettant l'API </summary>
         *  <param name="nomControleur">Controleur à requetter</param>
         *  <returns>Liste des produits</returns>
         */
        public async Task<List<Produit>> GetProduitsAsync(string? nomControleur)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<List<Produit>>(nomControleur);
                return response ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: ", ex);
                return [];
            } 
        }

        // POST: Add a new product
        public async Task<Produit> PostProduitAsync(string? nomControleur, Produit produit)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(nomControleur, produit);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Produit>();
                }
                else
                {
                    Console.WriteLine($"Failed to post product: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting product: {ex.Message}");
                return null;
            }
        }

        // PUT: Update an existing product
        public async Task<bool> PutProduitAsync(string? nomControleur, Produit produit)
        {
            try
            {
                var response = await _client.PutAsJsonAsync(nomControleur, produit);
                if (response.IsSuccessStatusCode)
                {
                    return true;  // Successfully updated
                }
                else
                {
                    Console.WriteLine($"Failed to update product: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }
    }
}
