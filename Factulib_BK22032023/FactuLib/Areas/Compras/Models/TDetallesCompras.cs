using FactuLib.Areas.Productos.Models;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Compras.Models
{
    public class TDetallesCompras
    {
        [Key]
        public int Id_Detalle_Compras { get; set; }

        public int Cantidad_Producto { get; set; }

        public float Monto_Bruto_Detalle { get; set; }

        public float Descuento_Detalle { get; set; }

        public float Monto_Neto_Detalle { get; set; }

        public float Monto_Impuesto_Detalle { get; set; }

        public TRegistroCompras TRegistroCompras { get; set; }

        public TProducto TProducto { get; set;  }

    }
}
