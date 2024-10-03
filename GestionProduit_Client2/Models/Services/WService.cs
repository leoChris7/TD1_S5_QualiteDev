// /Services/WSService.cs
using GestionProduit_Client.Models.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class WSService : IService
{
    private readonly HttpClient _httpClient;

    public WSService(String? Uri= "http://localhost:5012/api/")
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Uri);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
