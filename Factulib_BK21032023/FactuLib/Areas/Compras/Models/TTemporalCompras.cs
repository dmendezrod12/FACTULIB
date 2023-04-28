using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Users.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Compras.Models
{
    public class TTemporalCompras
    {
        [Key]
        public int idTempCompras { get; set; }

        public float TotalNeto { get; set; }

        public float TotalImpuestos { get; set; }

        public float TotalDescuentos { get; set; }

        public float TotalBruto { get; set; }

        public int Cantidad_Compra { get; set; }

        [ForeignKey("IdProveedor")]
        public TProveedor Tproveedor { get; set; }

        [ForeignKey("IdUser")]
        public TUser TUser { get; set; }

        [ForeignKey("IdProducto")]

        public TProducto TProducto { get; set; }

        public DateTime Date { get; set; }

    }
}
