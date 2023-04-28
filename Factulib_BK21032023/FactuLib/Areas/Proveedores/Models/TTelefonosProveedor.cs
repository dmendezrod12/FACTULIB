using FactuLib.Areas.Clientes.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Proveedores.Models
{
    public class TTelefonosProveedor
    {
        [Key]
        public int idTelefono { get; set; }


        public string telefono { get; set; }

        [ForeignKey("IdProveedor")]
        public TProveedor proveedor { get; set; }
    }
}
