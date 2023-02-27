using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total_Ventas",
                table: "TRegisroCierre",
                newName: "Total_Ventas_Efectivo");

            migrationBuilder.RenameColumn(
                name: "Total_Compras",
                table: "TRegisroCierre",
                newName: "Total_Ventas_Cuentas");

            migrationBuilder.RenameColumn(
                name: "Monto_Total_Cierre",
                table: "TRegisroCierre",
                newName: "Total_Ventas_Credito");

            migrationBuilder.AddColumn<float>(
                name: "Total_Compras_Credito",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Total_Compras_Cuentas",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Total_Compras_Efectivo",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total_Compras_Credito",
                table: "TRegisroCierre");

            migrationBuilder.DropColumn(
                name: "Total_Compras_Cuentas",
                table: "TRegisroCierre");

            migrationBuilder.DropColumn(
                name: "Total_Compras_Efectivo",
                table: "TRegisroCierre");

            migrationBuilder.RenameColumn(
                name: "Total_Ventas_Efectivo",
                table: "TRegisroCierre",
                newName: "Total_Ventas");

            migrationBuilder.RenameColumn(
                name: "Total_Ventas_Cuentas",
                table: "TRegisroCierre",
                newName: "Total_Compras");

            migrationBuilder.RenameColumn(
                name: "Total_Ventas_Credito",
                table: "TRegisroCierre",
                newName: "Monto_Total_Cierre");
        }
    }
}
