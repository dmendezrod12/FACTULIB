using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "TProducto",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "TProducto");
        }
    }
}
