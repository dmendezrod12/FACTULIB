using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "TTemporalCompras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "TTemporalCompras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "TTemporalCompras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "TTemporalCompras",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "TTemporalCompras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Precio",
                table: "TTemporalCompras",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
