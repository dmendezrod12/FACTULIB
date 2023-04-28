using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migragtion30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Monto_Faltante",
                table: "TRegisroCierre",
                newName: "Monto_Sobrante_Cuentas");

            migrationBuilder.AddColumn<float>(
                name: "Monto_Faltante_Cajas",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Monto_Faltante_Cuentas",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Monto_Sobrante_Cajas",
                table: "TRegisroCierre",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Monto_Faltante_Cajas",
                table: "TRegisroCierre");

            migrationBuilder.DropColumn(
                name: "Monto_Faltante_Cuentas",
                table: "TRegisroCierre");

            migrationBuilder.DropColumn(
                name: "Monto_Sobrante_Cajas",
                table: "TRegisroCierre");

            migrationBuilder.RenameColumn(
                name: "Monto_Sobrante_Cuentas",
                table: "TRegisroCierre",
                newName: "Monto_Faltante");
        }
    }
}
