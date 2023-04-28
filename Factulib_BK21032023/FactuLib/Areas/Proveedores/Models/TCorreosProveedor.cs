using FactuLib.Areas.Clientes.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Proveedores.Models
{
    public class TCorreosProveedor
    {
        [Key]
        public int idCorreo { get; set; }

        public string correo { get; set; }

        [ForeignKey("IdProveedor")]
        public TProveedor proveedor { get; set; }
    }
}
