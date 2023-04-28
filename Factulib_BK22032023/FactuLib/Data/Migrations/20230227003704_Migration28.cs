using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_User",
                table: "TRegistroCompras",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRegistroCompras_Id_User",
                table: "TRegistroCompras",
                column: "Id_User");

            migrationBuilder.AddForeignKey(
                name: "FK_TRegistroCompras_TUsers_Id_User",
                table: "TRegistroCompras",
                column: "Id_User",
                principalTable: "TUsers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRegistroCompras_TUsers_Id_User",
                table: "TRegistroCompras");

            migrationBuilder.DropIndex(
                name: "IX_TRegistroCompras_Id_User",
                table: "TRegistroCompras");

            migrationBuilder.DropColumn(
                name: "Id_User",
                table: "TRegistroCompras");
        }
    }
}
