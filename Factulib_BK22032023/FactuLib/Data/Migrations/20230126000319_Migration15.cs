using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "TTemporalCompras",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalCompras_IdProducto",
                table: "TTemporalCompras",
                column: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_TTemporalCompras_TProducto_IdProducto",
                table: "TTemporalCompras",
                column: "IdProducto",
                principalTable: "TProducto",
                principalColumn: "Id_Producto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TTemporalCompras_TProducto_IdProducto",
                table: "TTemporalCompras");

            migrationBuilder.DropIndex(
                name: "IX_TTemporalCompras_IdProducto",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "TTemporalCompras");
        }
    }
}
