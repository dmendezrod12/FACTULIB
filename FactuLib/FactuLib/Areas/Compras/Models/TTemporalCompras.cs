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

        [Required(ErrorMessage = "El campo Nombre Producto es Obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo cantidad es obligarorio")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El campo precio es obligatorio")]
        
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El Pago no es correcto")]
        public float Precio { get; set; }

        public float TotalNeto { get; set; }

        public float TotalImpuestos { get; set; }

        public float TotalDescuentos { get; set; }

        public float TotalBruto { get; set; }

        [ForeignKey("IdProveedor")]
        public TProveedor Tproveedor { get; set; }

        [ForeignKey("IdUser")]
        public TUser TUser { get; set; }

        [ForeignKey("IdProducto")]

        public TProducto TProducto { get; set; }

        public byte[] Image { get; set; }

        public DateTime Date { get; set; }
    }
}
