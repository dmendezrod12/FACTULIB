using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Productos.Models
{
    public class TProducto
    {
        [Key]
        public int Id_Producto { get; set; }

        public int Codigo_Producto { get; set; }

        public string Nombre_Producto { get; set; }

        public string Descripcion_Producto { get; set; }

        public int Cantidad_Producto { get; set; }
        
        public float Precio_Costo { get; set; }

        public float Precio_Venta { get; set; }

        public float Descuento_Producto { get; set; }

        public DateTime Fecha { get; set; }

        public byte[] Imagen { get; set; }

        [ForeignKey("Id_TipoProducto")]

        public TTipoProducto TTipoProducto { get; set; }

    }
}
