using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "TTemporalCompras",
                newName: "TotalNeto");

            migrationBuilder.AddColumn<float>(
                name: "TotalBruto",
                table: "TTemporalCompras",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalDescuentos",
                table: "TTemporalCompras",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalImpuestos",
                table: "TTemporalCompras",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBruto",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "TotalDescuentos",
                table: "TTemporalCompras");

            migrationBuilder.DropColumn(
                name: "TotalImpuestos",
                table: "TTemporalCompras");

            migrationBuilder.RenameColumn(
                name: "TotalNeto",
                table: "TTemporalCompras",
                newName: "Monto");
        }
    }
}
