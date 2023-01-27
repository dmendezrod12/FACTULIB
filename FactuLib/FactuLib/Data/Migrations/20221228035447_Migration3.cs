using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactuLib.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TClientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Credito = table.Column<bool>(type: "bit", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TClientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "THistoricoPagosClientes",
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
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CedulaCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THistoricoPagosClientes", x => x.IdPago);
                });

            migrationBuilder.CreateTable(
                name: "TProveedor",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CedJur = table.Column<long>(name: "Ced_Jur", type: "bigint", nullable: false),
                    NombreProveedor = table.Column<string>(name: "Nombre_Proveedor", type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TProveedor", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "TProvincia",
                columns: table => new
                {
                    idProvincia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreProvincia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TProvincia", x => x.idProvincia);
                });

            migrationBuilder.CreateTable(
              name: "TCanton",
              columns: table => new
              {
                  idCanton = table.Column<int>(type: "int", nullable: false),
                  nombreCanton = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  idProvincia = table.Column<int>(type: "int", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_TCanton", x => x.idCanton);
                  table.ForeignKey(
                      name: "FK_TCanton_TProvincia_idProvincia",
                      column: x => x.idProvincia,
                      principalTable: "TProvincia",
                      principalColumn: "idProvincia");
              });

            migrationBuilder.CreateTable(
                name: "TDistrito",
                columns: table => new
                {
                    idDistrito = table.Column<int>(type: "int", nullable: false),
                    nombreDistrito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idCanton = table.Column<int>(type: "int", nullable: true),
                    idProvincia = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDistrito", x => x.idDistrito);
                    table.ForeignKey(
                        name: "FK_TDistrito_TCanton_idCanton",
                        column: x => x.idCanton,
                        principalTable: "TCanton",
                        principalColumn: "idCanton");
                    table.ForeignKey(
                        name: "FK_TDistrito_TProvincia_idProvincia",
                        column: x => x.idProvincia,
                        principalTable: "TProvincia",
                        principalColumn: "idProvincia");
                });

            migrationBuilder.CreateTable(
                name: "TCorreosClientes",
                columns: table => new
                {
                    idCorreo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCorreosClientes", x => x.idCorreo);
                    table.ForeignKey(
                        name: "FK_TCorreosClientes_TClientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "TClientes",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "TCreditoClientes",
                columns: table => new
                {
                    idDeuda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deuda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UltimoPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeudaActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaDeuda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CedulaCliente = table.Column<int>(type: "int", nullable: false),
                    TClientsIdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCreditoClientes", x => x.idDeuda);
                    table.ForeignKey(
                        name: "FK_TCreditoClientes_TClientes_TClientsIdCliente",
                        column: x => x.TClientsIdCliente,
                        principalTable: "TClientes",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "TTelefonoCliente",
                columns: table => new
                {
                    idTelefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTelefonoCliente", x => x.idTelefono);
                    table.ForeignKey(
                        name: "FK_TTelefonoCliente_TClientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "TClientes",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "TCorreosProveedor",
                columns: table => new
                {
                    idCorreo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCorreosProveedor", x => x.idCorreo);
                    table.ForeignKey(
                        name: "FK_TCorreosProveedor_TProveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "TCreditoProveedor",
                columns: table => new
                {
                    idCredito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deuda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UltimoPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeudaActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaDeuda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCreditoProveedor", x => x.idCredito);
                    table.ForeignKey(
                        name: "FK_TCreditoProveedor_TProveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "TTelefonosProveedor",
                columns: table => new
                {
                    idTelefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTelefonosProveedor", x => x.idTelefono);
                    table.ForeignKey(
                        name: "FK_TTelefonosProveedor_TProveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "TDireccionCliente",
                columns: table => new
                {
                    idDireccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idDistrito = table.Column<int>(type: "int", nullable: false),
                    idCanton = table.Column<int>(type: "int", nullable: true),
                    idProvincia = table.Column<int>(type: "int", nullable: true),
                    Cedula = table.Column<int>(type: "int", nullable: false),
                    clientesIdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDireccionCliente", x => x.idDireccion);
                    table.ForeignKey(
                        name: "FK_TDireccionCliente_TCanton_idCanton",
                        column: x => x.idCanton,
                        principalTable: "TCanton",
                        principalColumn: "idCanton");
                    table.ForeignKey(
                        name: "FK_TDireccionCliente_TClientes_clientesIdCliente",
                        column: x => x.clientesIdCliente,
                        principalTable: "TClientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK_TDireccionCliente_TProvincia_idProvincia",
                        column: x => x.idProvincia,
                        principalTable: "TProvincia",
                        principalColumn: "idProvincia");
                    table.ForeignKey(
                        name: "FK_TDireccionCliente_TDistrito_idDistrito",
                        column: x => x.idDistrito,
                        principalTable: "TDistrito",
                        principalColumn: "idDistrito");
                });

            migrationBuilder.CreateTable(
                name: "TDireccionesProveedor",
                columns: table => new
                {
                    idDireccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idDistrito = table.Column<int>(type: "int", nullable: false),
                    idProvincia = table.Column<int>(type: "int", nullable: true),
                    idCanton = table.Column<int>(type: "int", nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDireccionesProveedor", x => x.idDireccion);
                    table.ForeignKey(
                        name: "FK_TDireccionesProveedor_TCanton_idCanton",
                        column: x => x.idCanton,
                        principalTable: "TCanton",
                        principalColumn: "idCanton");
                    table.ForeignKey(
                        name: "FK_TDireccionesProveedor_TProveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "TProveedor",
                        principalColumn: "IdProveedor");
                    table.ForeignKey(
                        name: "FK_TDireccionesProveedor_TProvincia_idProvincia",
                        column: x => x.idProvincia,
                        principalTable: "TProvincia",
                        principalColumn: "idProvincia");
                    table.ForeignKey(
                        name: "FK_TDireccionProveedor_TDistrito_idDistrito",
                        column: x => x.idDistrito,
                        principalTable: "TDistrito",
                        principalColumn: "idDistrito");
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_TCanton_idProvincia",
                table: "TCanton",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_TCorreosClientes_IdCliente",
                table: "TCorreosClientes",
                column: "IdCliente",
                unique: true,
                filter: "[IdCliente] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TCorreosProveedor_IdProveedor",
                table: "TCorreosProveedor",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_TCreditoClientes_TClientsIdCliente",
                table: "TCreditoClientes",
                column: "TClientsIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_TCreditoProveedor_IdProveedor",
                table: "TCreditoProveedor",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionCliente_clientesIdCliente",
                table: "TDireccionCliente",
                column: "clientesIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionCliente_idCanton",
                table: "TDireccionCliente",
                column: "idCanton");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionCliente_idProvincia",
                table: "TDireccionCliente",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionesProveedor_idCanton",
                table: "TDireccionesProveedor",
                column: "idCanton");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionesProveedor_IdProveedor",
                table: "TDireccionesProveedor",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_TDireccionesProveedor_idProvincia",
                table: "TDireccionesProveedor",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_TDistrito_idCanton",
                table: "TDistrito",
                column: "idCanton");

            migrationBuilder.CreateIndex(
                name: "IX_TDistrito_idProvincia",
                table: "TDistrito",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_TTelefonoCliente_IdCliente",
                table: "TTelefonoCliente",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_TTelefonosProveedor_IdProveedor",
                table: "TTelefonosProveedor",
                column: "IdProveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TCorreosClientes");

            migrationBuilder.DropTable(
                name: "TCorreosProveedor");

            migrationBuilder.DropTable(
                name: "TCreditoClientes");

            migrationBuilder.DropTable(
                name: "TCreditoProveedor");

            migrationBuilder.DropTable(
                name: "TDireccionCliente");

            migrationBuilder.DropTable(
                name: "TDireccionesProveedor");

            migrationBuilder.DropTable(
                name: "TDistrito");

            migrationBuilder.DropTable(
                name: "THistoricoPagosClientes");

            migrationBuilder.DropTable(
                name: "TTelefonoCliente");

            migrationBuilder.DropTable(
                name: "TTelefonosProveedor");

            migrationBuilder.DropTable(
                name: "TCanton");

            migrationBuilder.DropTable(
                name: "TClientes");

            migrationBuilder.DropTable(
                name: "TProveedor");

            migrationBuilder.DropTable(
                name: "TProvincia");
        }
    }
}
