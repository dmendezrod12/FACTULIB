using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TTemporalCompras",
                columns: table => new
                {
                    idTempCompras = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: true),
                    IdUser = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTemporalCompras", x => x.idTempCompras);
                    table.ForeignKey(
                        name: "FK_TTemporalCompras_TProveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                    table.ForeignKey(
                        name: "FK_TTemporalCompras_TUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "TUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalCompras_IdProveedor",
                table: "TTemporalCompras",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_TTemporalCompras_IdUser",
                table: "TTemporalCompras",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TTemporalCompras");
        }
    }
}
