using FactuLib.Areas.Proveedores.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Users.Models;

namespace FactuLib.Areas.Ventas.Models
{
    public class TRegistroVentas
    {
        [Key]
        public int Id_RegistroVentas { get; set; }

        public float Total_Bruto { get; set; }

        public float Total_Descuento { get; set; }

        public float Total_IVA { get; set; }

        public float Total_Neto { get; set; }

        public int MetodoPago { get; set; }

        public float DineroRecibido { get; set; }

        public Decimal CambioCompra { get; set; }

        public DateTime Fecha_Compra { get; set; }

        public bool Estado_Venta { get; set; }

        [ForeignKey("Id_Cliente")]
        public TClientes TClientes { get; set; }

        [ForeignKey("Id_User")]

        public TUser TUser { get; set; }
    }
}
