using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRegistroVentas",
                columns: table => new
                {
                    IdRegistroVentas = table.Column<int>(name: "Id_RegistroVentas", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalBruto = table.Column<float>(name: "Total_Bruto", type: "real", nullable: false),
                    TotalDescuento = table.Column<float>(name: "Total_Descuento", type: "real", nullable: false),
                    TotalIVA = table.Column<float>(name: "Total_IVA", type: "real", nullable: false),
                    TotalNeto = table.Column<float>(name: "Total_Neto", type: "real", nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false),
                    DineroRecibido = table.Column<float>(type: "real", nullable: false),
                    CambioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCompra = table.Column<DateTime>(name: "Fecha_Compra", type: "datetime2", nullable: false),
                    IdCliente = table.Column<int>(name: "Id_Cliente", type: "int", nullable: true),
                    IdUser = table.Column<int>(name: "Id_User", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRegistroVentas", x => x.IdRegistroVentas);
                    table.ForeignKey(
                        name: "FK_TRegistroVentas_TClientes_Id_Cliente",
                        column: x => x.IdCliente,
                        principalTable: "TClientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK_TRegistroVentas_TUsers_Id_User",
                        column: x => x.IdUser,
                        principalTable: "TUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TTemporalVentas",
                columns: table => new
                {
                    idTempVentas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalNeto = table.Column<float>(type: "real", nullable: false),
                    TotalImpuestos = table.Column<float>(type: "real", nullable: false),
                    TotalDescuentos = table.Column<float>(type: "real", nullable: false),
                    TotalBruto = table.Column<float>(type: "real", nullable: false),
                    CantidadVenta = table.Column<int>(name: "Cantidad_Venta", type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    IdUser = table.Column<int>(type: "int", nullable: true),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTemporalVentas", x => x.idTempVentas);
                    table.ForeignKey(
                        name: "FK_TTemporalVentas_TProducto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "TProducto",
                        principalColumn: "Id_Producto");
                    table.ForeignKey(
                        name: "FK_TTemporalVentas_TProveedor_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                    table.ForeignKey(
                        name: "FK_TTemporalVentas_TUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "TUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TDetallesVentas",
                columns: table => new
                {
                    IdDetalleVenta = table.Column<int>(name: "Id_Detalle_Venta", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadProducto = table.Column<int>(name: "Cantidad_Producto", type: "int", nullable: false),
                    MontoBrutoDetalle = table.Column<float>(name: "Monto_Bruto_Detalle", type: "real", nullable: false),
                    DescuentoDetalle = table.Column<float>(name: "Descuento_Detalle", type: "real", nullable: false),
                    MontoNetoDetalle = table.Column<float>(name: "Monto_Neto_Detalle", type: "real", nullable: false),
                    MontoImpuestoDetalle = table.Column<float>(name: "Monto_Impuesto_Detalle", type: "real", nullable: false),
                    TRegistroVentasIdRegistroVentas = table.Column<int>(name: "TRegistroVentasId_RegistroVentas", type: "int", nullable: true),
                    TProductoIdProducto = table.Column<int>(name: "TProductoId_Producto", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDetallesVentas", x => x.IdDetalleVenta);
                    table.ForeignKey(
                        name: "FK_TDetallesVentas_TProducto_TProductoId_Producto",
                        column: x => x.TProductoIdProducto,
                        principalTable: "TProducto",
                        principalColumn: "Id_Producto");
                    table.ForeignKey(
                        name: "FK_TDetallesVentas_TRegistroVentas_TRegistroVentasId_RegistroVentas",
                        column: x => x.TRegistroVentasIdRegistroVentas,
                        principalTable: "TRegistroVentas",
                        principalColumn: "Id_RegistroVentas");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TDetallesVentas_TProductoId_Producto",
                table: "TDetallesVentas",
                column: "TProductoId_Producto");

            migrationBuilder.CreateIndex(
                name: "IX_TDetallesVentas_TRegistroVentasId_RegistroVentas",
                table: "TDetallesVentas",
                column: "TRegistroVentasId_RegistroVentas");

            migrationBuilder.CreateIndex(
                name: "IX_TRegistroVentas_Id_Cliente",
                table: "TRegistroVentas",
                column: "Id_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_TRegistroVentas_Id_User",
                table: "TRegistroVentas",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalVentas_IdCliente",
                table: "TTemporalVentas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalVentas_IdProducto",
                table: "TTemporalVentas",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalVentas_IdUser",
                table: "TTemporalVentas",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TDetallesVentas");

            migrationBuilder.DropTable(
                name: "TTemporalVentas");

            migrationBuilder.DropTable(
                name: "TRegistroVentas");
        }
    }
}
