using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Ventas.Models
{
    public class TDetallesVentas
    {
        [Key]
        public int Id_Detalle_Venta { get; set; }

        public int Cantidad_Producto { get; set; }

        public float Monto_Bruto_Detalle { get; set; }

        public float Descuento_Detalle { get; set; }

        public float Monto_Neto_Detalle { get; set; }

        public float Monto_Impuesto_Detalle { get; set; }
        
        public TRegistroVentas TRegistroVentas { get; set; }

        public TProducto TProducto { get; set; }

    }
}
