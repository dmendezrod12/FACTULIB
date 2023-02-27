using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado_Venta",
                table: "TRegistroVentas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Estado_Compra",
                table: "TRegistroCompras",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TRegisroApertura",
                columns: table => new
                {
                    IdApertura = table.Column<int>(name: "Id_Apertura", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(name: "Id_Usuario", type: "int", nullable: true),
                    FechaApertura = table.Column<DateTime>(name: "Fecha_Apertura", type: "datetime2", nullable: false),
                    DineroCajas = table.Column<float>(name: "Dinero_Cajas", type: "real", nullable: false),
                    DineroCuentas = table.Column<float>(name: "Dinero_Cuentas", type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRegisroApertura", x => x.IdApertura);
                    table.ForeignKey(
                        name: "FK_TRegisroApertura_TUsers_Id_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "TUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TRegisroCierre",
                columns: table => new
                {
                    IdCierre = table.Column<int>(name: "Id_Cierre", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(name: "Id_Usuario", type: "int", nullable: true),
                    IdApertura = table.Column<int>(name: "Id_Apertura", type: "int", nullable: true),
                    FechaCierre = table.Column<DateTime>(name: "Fecha_Cierre", type: "datetime2", nullable: false),
                    DineroCajas = table.Column<float>(name: "Dinero_Cajas", type: "real", nullable: false),
                    DineroCuentas = table.Column<float>(name: "Dinero_Cuentas", type: "real", nullable: false),
                    TotalVentas = table.Column<float>(name: "Total_Ventas", type: "real", nullable: false),
                    TotalCompras = table.Column<float>(name: "Total_Compras", type: "real", nullable: false),
                    MontoTotalCierre = table.Column<float>(name: "Monto_Total_Cierre", type: "real", nullable: false),
                    MontoFaltante = table.Column<float>(name: "Monto_Faltante", type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRegisroCierre", x => x.IdCierre);
                    table.ForeignKey(
                        name: "FK_TRegisroCierre_TRegisroApertura_Id_Apertura",
                        column: x => x.IdApertura,
                        principalTable: "TRegisroApertura",
                        principalColumn: "Id_Apertura");
                    table.ForeignKey(
                        name: "FK_TRegisroCierre_TUsers_Id_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "TUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRegisroApertura_Id_Usuario",
                table: "TRegisroApertura",
                column: "Id_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_TRegisroCierre_Id_Apertura",
                table: "TRegisroCierre",
                column: "Id_Apertura");

            migrationBuilder.CreateIndex(
                name: "IX_TRegisroCierre_Id_Usuario",
                table: "TRegisroCierre",
                column: "Id_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRegisroCierre");

            migrationBuilder.DropTable(
                name: "TRegisroApertura");

            migrationBuilder.DropColumn(
                name: "Estado_Venta",
                table: "TRegistroVentas");

            migrationBuilder.DropColumn(
                name: "Estado_Compra",
                table: "TRegistroCompras");
        }
    }
}
