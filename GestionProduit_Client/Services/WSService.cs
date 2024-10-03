using GestionProduit_Client.Models;
using GestionProduit_Client.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionProduit_Client.Services
{
    public class WSService:IService
    {
        HttpClient? _client;

        public WSService(String? Uri = "http://localhost:8015/api/")
        {
            this._client = new HttpClient();
            this._client.BaseAddress = new Uri(Uri);
            this._client.DefaultRequestHeaders.Accept.Clear();
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Produit>> GetProduitsAsync(String? nomControleur)
        {
            try
            {
                var _response =  await _client.GetFromJsonAsync<List<Produit>>(nomControleur);
                return _response;

            } catch (Exception)
            {
                return null;
            }
        }
    }
}
