﻿// <auto-generated />
using GestionProduit_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionProduit_API.Migrations
{
    [DbContext(typeof(ProduitDbContext))]
    [Migration("20240918082125_bd")]
    partial class bd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TPRevision.Models.EntityFramework.Marque", b =>
                {
                    b.Property<int>("Idmarque")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idmarque");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Idmarque"));

                    b.Property<string>("NomMarque")
                        .HasColumnType("text")
                        .HasColumnName("nommarque");

                    b.HasKey("Idmarque")
                        .HasName("pk_mrq");

                    b.ToTable("marque");
                });

            modelBuilder.Entity("TPRevision.Models.EntityFramework.Produit", b =>
                {
                    b.Property<int>("IdProduit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idproduit");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdProduit"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("IdMarque")
                        .HasColumnType("integer");

                    b.Property<int>("IdTypeProduit")
                        .HasColumnType("integer");

                    b.Property<string>("NomPhoto")
                        .HasColumnType("text")
                        .HasColumnName("nomphoto");

                    b.Property<string>("NomProduit")
                        .HasColumnType("text")
                        .HasColumnName("nomproduit");

                    b.Property<int>("StockMax")
                        .HasColumnType("integer")
                        .HasColumnName("stockmax");

                    b.Property<int>("StockMin")
                        .HasColumnType("integer")
                        .HasColumnName("stockmin");

                    b.Property<int>("StockReel")
                        .HasColumnType("integer")
                        .HasColumnName("stockreel");

                    b.Property<string>("UriPhoto")
                        .HasColumnType("text")
                        .HasColumnName("uriphoto");

                    b.HasKey("IdProduit")
                        .HasName("pk_pdt");

                    b.HasIndex("IdMarque");

                    b.HasIndex("IdTypeProduit");

                    b.ToTable("produit");
                });

            modelBuilder.Entity("TPRevision.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Property<int>("Idtypeproduit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idtypeproduit");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Idtypeproduit"));

                    b.Property<string>("nomtypeproduit")
                        .HasColumnType("text")
                        .HasColumnName("nomtypeproduit");

                    b.HasKey("Idtypeproduit")
                        .HasName("pk_typepdt");

                    b.ToTable("typeproduit");
                });

            modelBuilder.Entity("TPRevision.Models.EntityFramework.Produit", b =>
                {
                    b.HasOne("TPRevision.Models.EntityFramework.Marque", "IdMarqueNavigation")
                        .WithMany("Produits")
                        .HasForeignKey("IdMarque")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_pdt_mrq");

                    b.HasOne("TPRevision.Models.EntityFramework.TypeProduit", "IdTypeProduitNavigation")
                        .WithMany("Produits")
                        .HasForeignKey("IdTypeProduit")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_pdt_typ");

                    b.Navigation("IdMarqueNavigation");

                    b.Navigation("IdTypeProduitNavigation");
                });

            modelBuilder.Entity("TPRevision.Models.EntityFramework.Marque", b =>
                {
                    b.Navigation("Produits");
                });

            modelBuilder.Entity("TPRevision.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Navigation("Produits");
                });
#pragma warning restore 612, 618
        }
    }
}
