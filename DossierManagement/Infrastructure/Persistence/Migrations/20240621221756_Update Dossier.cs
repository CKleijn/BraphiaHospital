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
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Dossiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Consults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Consults");
        }
    }
}
