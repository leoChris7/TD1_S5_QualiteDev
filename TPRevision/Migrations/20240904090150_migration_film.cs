using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace tprevision.Migrations
{
    /// <inheritdoc />
    public partial class migration_film : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marque",
                columns: table => new
                {
                    idmarque = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nommarque = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mrq", x => x.idmarque);
                });

            migrationBuilder.CreateTable(
                name: "typeproduit",
                columns: table => new
                {
                    idtypeproduit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomtypeproduit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_typepdt", x => x.idtypeproduit);
                });

            migrationBuilder.CreateTable(
                name: "produit",
                columns: table => new
                {
                    idproduit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomproduit = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    nomphoto = table.Column<string>(type: "text", nullable: true),
                    uriphoto = table.Column<string>(type: "text", nullable: true),
                    IdTypeProduit = table.Column<int>(type: "integer", nullable: false),
                    IdMarque = table.Column<int>(type: "integer", nullable: false),
                    stockreel = table.Column<int>(type: "integer", nullable: false),
                    stockmin = table.Column<int>(type: "integer", nullable: false),
                    stockmax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pdt", x => x.idproduit);
                    table.ForeignKey(
                        name: "fk_pdt_mrq",
                        column: x => x.IdMarque,
                        principalTable: "marque",
                        principalColumn: "idmarque",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pdt_typ",
                        column: x => x.IdTypeProduit,
                        principalTable: "typeproduit",
                        principalColumn: "idtypeproduit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produit_IdMarque",
                table: "produit",
                column: "IdMarque");

            migrationBuilder.CreateIndex(
                name: "IX_produit_IdTypeProduit",
                table: "produit",
                column: "IdTypeProduit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produit");

            migrationBuilder.DropTable(
                name: "marque");

            migrationBuilder.DropTable(
                name: "typeproduit");
        }
    }
}
