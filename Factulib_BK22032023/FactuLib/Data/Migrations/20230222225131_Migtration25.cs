using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migtration25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TTemporalVentas_TProveedor_IdCliente",
                table: "TTemporalVentas");

            migrationBuilder.AddForeignKey(
                name: "FK_TTemporalVentas_TClientes_IdCliente",
                table: "TTemporalVentas",
                column: "IdCliente",
                principalTable: "TClientes",
                principalColumn: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TTemporalVentas_TClientes_IdCliente",
                table: "TTemporalVentas");

            migrationBuilder.AddForeignKey(
                name: "FK_TTemporalVentas_TProveedor_IdCliente",
                table: "TTemporalVentas",
                column: "IdCliente",
                principalTable: "TProveedor",
                principalColumn: "IdProveedor");
        }
    }
}
