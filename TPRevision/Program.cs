using Microsoft.EntityFrameworkCore;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.ModelTemplate;
using GestionProduit_API.Models.DTO;
using GestionProduit_API;

var MyAllowSpecificOrigins = "AllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("https://localhost:7245") // Allow this origin
               .AllowAnyMethod()                     // Allow any HTTP methods
               .AllowAnyHeader();                    // Allow any headers
    });
});


// Récupérer la chaîne de connexion depuis le fichier de configuration
var connectionString = builder.Configuration.GetConnectionString("ProduitDbConnection");

// Ajouter les services à l'injection de dépendance
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
builder.Services.AddDbContext<ProduitDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

// Ajouter les services Scoped pour les managers
builder.Services.AddScoped<MarqueManager>();
builder.Services.AddScoped<TypeProduitManager>();
builder.Services.AddScoped<ProduitManager>();

// Configurer Swagger pour la documentation de l'API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//builder.Services.AddAutoMapper(cfg => {
//    cfg.CreateMap<Produit, ProduitSansNavigation>().ReverseMap();
//    cfg.CreateMap<Produit, ProduitDTO>().ReverseMap();
//    cfg.CreateMap<Produit, ProduitDetailDTO>().ReverseMap();
//    cfg.CreateMap<Marque, MarqueDTO>().ReverseMap();
//    cfg.CreateMap<TypeProduit, TypeProduitDTO>().ReverseMap();
//});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configurer le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
