using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DossierManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Consults",
                newName: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Consults",
                newName: "Note");
        }
    }
}
