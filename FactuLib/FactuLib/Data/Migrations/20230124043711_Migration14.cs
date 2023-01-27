using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio_Producto",
                table: "TProducto",
                newName: "Precio_Venta");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion_Producto",
                table: "TProducto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Precio_Costo",
                table: "TProducto",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion_Producto",
                table: "TProducto");

            migrationBuilder.DropColumn(
                name: "Precio_Costo",
                table: "TProducto");

            migrationBuilder.RenameColumn(
                name: "Precio_Venta",
                table: "TProducto",
                newName: "Precio_Producto");
        }
    }
}
