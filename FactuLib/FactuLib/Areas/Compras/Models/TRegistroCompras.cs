using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Users.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Compras.Models
{
    public class TRegistroCompras
    {
        [Key]
        public int Id_RegistroCompras { get; set; }

        public string NumeroFacturaProveedor { get; set; }

        public float Total_Bruto { get; set; }

        public float Total_Descuento { get; set; }

        public float Total_IVA { get; set; }

        public float Total_Neto { get; set; }

        public int MetodoPago { get; set; }

        public float DineroRecibido { get; set; }

        public Decimal CambioCompra { get; set; }

        public DateTime Fecha_Compra { get; set; }

        public bool Estado_Compra { get; set; }

        [ForeignKey("Id_Proveedor")]
        public TProveedor TProveedor { get; set; }

        [ForeignKey("Id_User")]

        public TUser TUser { get; set; }
    }
}
