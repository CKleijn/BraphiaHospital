using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DossierManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddDossierrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consults_Dossiers_DossierId",
                table: "Consults");

            migrationBuilder.AlterColumn<Guid>(
                name: "DossierId",
                table: "Consults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consults_Dossiers_DossierId",
                table: "Consults",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consults_Dossiers_DossierId",
                table: "Consults");

            migrationBuilder.AlterColumn<Guid>(
                name: "DossierId",
                table: "Consults",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Consults_Dossiers_DossierId",
                table: "Consults",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id");
        }
    }
}
