using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace FactuLib.Areas.Productos.Models
{
    public class InputModelProductos
    {
        public int Id_Producto { get; set; }
        public int Codigo_Producto { get; set; }

        public string Nombre_Producto { get; set; }

        public string Descripcion_Producto { get; set; }

        public int Cantidad_Producto { get; set; }

        public string Precio_Costo { get; set; }

        public string Precio_Venta { get; set; }

        public int Descuento_Producto { get; set; }

        public DateTime Fecha { get; set; }

        public byte[] Imagen { get; set; }

        [ForeignKey("Id_TipoProducto")]

        public TTipoProducto TTipoProducto { get; set; }

        public string ErrorMessage { get; set; }
    }
}
