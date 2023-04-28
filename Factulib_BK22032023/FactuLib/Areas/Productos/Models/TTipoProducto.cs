using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Productos.Models
{
    public class TTipoProducto
    {
        [Key]
        public int Id_TipoProducto { get; set; }

        public string NombreTipo { get; set; }
    }
}
