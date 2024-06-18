using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DossierManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDossier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medications",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "Results",
                table: "Dossiers");

            migrationBuilder.CreateTable(
                name: "Medication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Medicine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DossierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medication_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DossierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Result_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medication_DossierId",
                table: "Medication",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_DossierId",
                table: "Result",
                column: "DossierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medication");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.AddColumn<string>(
                name: "Medications",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Results",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
