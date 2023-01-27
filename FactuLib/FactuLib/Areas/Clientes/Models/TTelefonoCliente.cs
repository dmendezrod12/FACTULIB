
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Clientes.Models
{
    public class TTelefonoCliente
    {
        [Key]
        public int idTelefono { get; set; }


        public string telefono { get; set;}

        [ForeignKey("IdCliente")]
        public TClientes clientes { get; set; }
    }
}
