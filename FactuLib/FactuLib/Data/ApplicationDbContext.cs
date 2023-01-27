using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FactuLib.Areas.Users.Models;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;

namespace FactuLib.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        static DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContext() : base(_options)
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<TUser> TUsers { get; set; }

        public DbSet<TClientes> TClientes { get; set; }

        public DbSet <THistoricoPagosClientes> THistoricoPagosClientes { get; set; }

        public DbSet <TProvincia> TProvincia { get; set; }

        public DbSet <TCanton> TCanton { get; set; }

        public DbSet <TDistrito> TDistrito { get; set; }

        public DbSet<TDireccionCliente> TDireccionCliente { get; set; }

        public DbSet <TCorreosClientes> TCorreosClientes { get; set; }

        public DbSet <TTelefonoCliente> TTelefonoCliente {get; set; }

        public DbSet <TCreditoClientes> TCreditoClientes { get; set; }

        public DbSet <TProveedor> TProveedor { get; set; }

        public DbSet <TCreditoProveedor> TCreditoProveedor { get; set; }

        public DbSet <TCorreosProveedor> TCorreosProveedor { get; set; }

        public DbSet <TTelefonosProveedor> TTelefonosProveedor { get; set; }

        public DbSet <TDireccionesProveedor> TDireccionesProveedor {get; set; }

        public DbSet <THistoricoPagosProveedor> THistoricoPagosProveedor { get; set; }

        public DbSet <TTemporalCompras> TTemporalCompras { get; set; }

        public DbSet <TRegistroCompras> TRegistroCompras { get; set; }

        public DbSet <TDetallesCompras> TDetallesCompras { get; set; }

        public DbSet <TTipoProducto> TTipoProducto { get; set; }

        public DbSet <TProducto> TProducto { get; set; }

        //public DbSet <TTemporalProducto> TTemporalProducto { get; set; }
    }
}
