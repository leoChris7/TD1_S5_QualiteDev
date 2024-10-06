using GestionProduit_Client.Services;
using GestionProduit_Client.Services.Interfaces;
using GestionProduit_Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace GestionProduit_Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<IService, WSService>();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Injection du ViewModel
            builder.Services.AddScoped<ProduitViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
