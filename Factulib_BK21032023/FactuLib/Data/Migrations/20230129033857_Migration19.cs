using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CambioCompra",
                table: "TRegistroCompras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DineroRecibido",
                table: "TRegistroCompras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MetodoPago",
                table: "TRegistroCompras",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CambioCompra",
                table: "TRegistroCompras");

            migrationBuilder.DropColumn(
                name: "DineroRecibido",
                table: "TRegistroCompras");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "TRegistroCompras");
        }
    }
}
