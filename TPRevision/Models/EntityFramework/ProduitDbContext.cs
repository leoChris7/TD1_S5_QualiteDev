﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TPRevision.Models.EntityFramework
{
    public partial class ProduitDbContext : DbContext
    {
        public ProduitDbContext() { }
        public ProduitDbContext(DbContextOptions<ProduitDbContext>options): base(options) { }

        public virtual DbSet<Produit> Produits { get; set; } = null!;
        public virtual DbSet<TypeProduit> Types { get; set; } = null!;
        public virtual DbSet<Marque> Marques { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=FilmDB; uid=postgres; password=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(e => e.IdProduit)
                    .HasName("pk_pdt");

                entity.HasOne(d => d.IdMarqueNavigation)
                    .WithMany(p => p.Produits)
                    .HasForeignKey(d => d.IdMarque)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_pdt_mrq");

                entity.HasOne(d => d.IdTypeProduitNavigation)
                    .WithMany(p => p.Produits)
                    .HasForeignKey(d => d.IdTypeProduit)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_pdt_typ");
            });

            modelBuilder.Entity<TypeProduit>(entity =>
            {
                entity.HasKey(e => e.Idtypeproduit)
                    .HasName("pk_typepdt");
            });

            modelBuilder.Entity<Marque>(entity =>
            {
                entity.HasKey(e => e.Idmarque)
                    .HasName("pk_mrq");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}