using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRegistroCompras",
                columns: table => new
                {
                    IdRegistroCompras = table.Column<int>(name: "Id_RegistroCompras", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroFacturaProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBruto = table.Column<float>(name: "Total_Bruto", type: "real", nullable: false),
                    TotalDescuento = table.Column<float>(name: "Total_Descuento", type: "real", nullable: false),
                    TotalIVA = table.Column<float>(name: "Total_IVA", type: "real", nullable: false),
                    TotalNeto = table.Column<float>(name: "Total_Neto", type: "real", nullable: false),
                    FechaCompra = table.Column<DateTime>(name: "Fecha_Compra", type: "datetime2", nullable: false),
                    IdProveedor = table.Column<int>(name: "Id_Proveedor", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRegistroCompras", x => x.IdRegistroCompras);
                    table.ForeignKey(
                        name: "FK_TRegistroCompras_TProveedor_Id_Proveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "TTipoProducto",
                columns: table => new
                {
                    IdTipoProducto = table.Column<int>(name: "Id_TipoProducto", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTipoProducto", x => x.IdTipoProducto);
                });

            migrationBuilder.CreateTable(
                name: "TProducto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(name: "Id_Producto", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoProducto = table.Column<int>(name: "Codigo_Producto", type: "int", nullable: false),
                    NombreProducto = table.Column<string>(name: "Nombre_Producto", type: "nvarchar(max)", nullable: true),
                    CantidadProducto = table.Column<int>(name: "Cantidad_Producto", type: "int", nullable: false),
                    PrecioProducto = table.Column<float>(name: "Precio_Producto", type: "real", nullable: false),
                    DescuentoProducto = table.Column<float>(name: "Descuento_Producto", type: "real", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdTipoProducto = table.Column<int>(name: "Id_TipoProducto", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TProducto", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_TProducto_TTipoProducto_Id_TipoProducto",
                        column: x => x.IdTipoProducto,
                        principalTable: "TTipoProducto",
                        principalColumn: "Id_TipoProducto");
                });

            migrationBuilder.CreateTable(
                name: "TDetallesCompras",
                columns: table => new
                {
                    IdDetalleCompras = table.Column<int>(name: "Id_Detalle_Compras", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadProducto = table.Column<int>(name: "Cantidad_Producto", type: "int", nullable: false),
                    MontoBrutoDetalle = table.Column<float>(name: "Monto_Bruto_Detalle", type: "real", nullable: false),
                    DescuentoDetalle = table.Column<float>(name: "Descuento_Detalle", type: "real", nullable: false),
                    MontoNetoDetalle = table.Column<float>(name: "Monto_Neto_Detalle", type: "real", nullable: false),
                    TRegistroComprasIdRegistroCompras = table.Column<int>(name: "TRegistroComprasId_RegistroCompras", type: "int", nullable: true),
                    TProductoIdProducto = table.Column<int>(name: "TProductoId_Producto", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDetallesCompras", x => x.IdDetalleCompras);
                    table.ForeignKey(
                        name: "FK_TDetallesCompras_TProducto_TProductoId_Producto",
                        column: x => x.TProductoIdProducto,
                        principalTable: "TProducto",
                        principalColumn: "Id_Producto");
                    table.ForeignKey(
                        name: "FK_TDetallesCompras_TRegistroCompras_TRegistroComprasId_RegistroCompras",
                        column: x => x.TRegistroComprasIdRegistroCompras,
                        principalTable: "TRegistroCompras",
                        principalColumn: "Id_RegistroCompras");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TDetallesCompras_TProductoId_Producto",
                table: "TDetallesCompras",
                column: "TProductoId_Producto");

            migrationBuilder.CreateIndex(
                name: "IX_TDetallesCompras_TRegistroComprasId_RegistroCompras",
                table: "TDetallesCompras",
                column: "TRegistroComprasId_RegistroCompras");

            migrationBuilder.CreateIndex(
                name: "IX_TProducto_Id_TipoProducto",
                table: "TProducto",
                column: "Id_TipoProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TRegistroCompras_Id_Proveedor",
                table: "TRegistroCompras",
                column: "Id_Proveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TDetallesCompras");

            migrationBuilder.DropTable(
                name: "TProducto");

            migrationBuilder.DropTable(
                name: "TRegistroCompras");

            migrationBuilder.DropTable(
                name: "TTipoProducto");
        }
    }
}
