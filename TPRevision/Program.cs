using Microsoft.EntityFrameworkCore;
using tprevision.Models.Manager;
using tprevision.Models.Manager.tprevision.Models.Manager;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDataRepository<Produit>, ProduitManager>();
builder.Services.AddScoped<IDataRepository<Marque>, MarqueManager>();
builder.Services.AddScoped<IDataRepository<TypeProduit>, TypeProduitManager>();

// Add services to the container.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
builder.Services.AddDbContext<ProduitDbContext>(options => options.UseNpgsql("Server=localhost;port=5432;Database=FilmDB;uid=postgres;password=postgres;"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
