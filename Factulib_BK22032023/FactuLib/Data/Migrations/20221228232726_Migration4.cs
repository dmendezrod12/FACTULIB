using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TCorreosClientes_IdCliente",
                table: "TCorreosClientes");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "TClientes",
                newName: "Apellido1");

            migrationBuilder.AddColumn<string>(
                name: "Apellido2",
                table: "TClientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TCorreosClientes_IdCliente",
                table: "TCorreosClientes",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TCorreosClientes_IdCliente",
                table: "TCorreosClientes");

            migrationBuilder.DropColumn(
                name: "Apellido1",
                table: "TClientes");

            migrationBuilder.RenameColumn(
                name: "Apellido2",
                table: "TClientes",
                newName: "Apellido");

            migrationBuilder.CreateIndex(
                name: "IX_TCorreosClientes_IdCliente",
                table: "TCorreosClientes",
                column: "IdCliente",
                unique: true,
                filter: "[IdCliente] IS NOT NULL");
        }
    }
}
