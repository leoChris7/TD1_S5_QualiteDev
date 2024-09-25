using Microsoft.EntityFrameworkCore;
using tprevision.Models.DataManager;
using tprevision.Models.DataManager;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
builder.Services.AddDbContext<ProduitDbContext>(options => options.UseNpgsql("Server=localhost;port=5432;Database=FilmDB;uid=postgres;password=postgres;"));
builder.Services.AddControllers();

builder.Services.AddScoped<MarqueManager>();
builder.Services.AddScoped<TypeProduitManager>();
builder.Services.AddScoped<ProduitManager>();

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
