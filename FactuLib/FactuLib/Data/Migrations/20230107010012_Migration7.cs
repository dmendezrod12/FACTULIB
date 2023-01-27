using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "THistoricoPagosProveedor",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deuda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeudaActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDeuda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeudaAnterior = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THistoricoPagosProveedor", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_THistoricoPagosProveedor_TProveedor_idProveedor",
                        column: x => x.idProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateIndex(
                name: "IX_THistoricoPagosProveedor_idProveedor",
                table: "THistoricoPagosProveedor",
                column: "idProveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "THistoricoPagosProveedor");
        }
    }
}
