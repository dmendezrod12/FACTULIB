using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Users.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using FactuLib.Areas.Clientes.Models;

namespace FactuLib.Areas.Ventas.Models
{
    public class TTemporalVentas
    {

        [Key]
        public int idTempVentas { get; set; }

        public float TotalNeto { get; set; }

        public float TotalImpuestos { get; set; }

        public float TotalDescuentos { get; set; }

        public float TotalBruto { get; set; }

        public int Cantidad_Venta { get; set; }

        [ForeignKey("IdCliente")]
        public TClientes TCliente { get; set; }

        [ForeignKey("IdUser")]
        public TUser TUser { get; set; }

        [ForeignKey("IdProducto")]

        public TProducto TProducto { get; set; }

        public DateTime Date { get; set; }
    }
}
