using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "TUsers",
                newName: "Apellido1");

            migrationBuilder.AddColumn<string>(
                name: "Apellido2",
                table: "TUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido2",
                table: "TUsers");

            migrationBuilder.RenameColumn(
                name: "Apellido1",
                table: "TUsers",
                newName: "LastName");
        }
    }
}
