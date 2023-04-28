using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Clientes.Models
{
    public class TCorreosClientes
    {
        [Key]
        public int idCorreo { get; set; }

        public string correo { get; set; }

        [ForeignKey ("IdCliente")]
        public TClientes cliente { get; set; }

    }
}
