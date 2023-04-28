using FactuLib.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Clientes.Models
{
    public class TDireccionCliente
    {
        [Key]
        public int idDireccion { get; set; }

        public string Direccion { get; set; }

        /*
        public int idProvincia { get; set; } 

        public int idCanton { get; set; }

        public int idDistrito { get; set; }
        */

        // [ForeignKey ("idProvincia")]

        //public TProvincia Provincia { get; set; }

        //[ForeignKey("idCanton")]

        //public TCanton Canton { get; set; }

        [ForeignKey("idDistrito")]

        public int idDistrito { get; set; }

        [ForeignKey("idCanton")]
        public TCanton TCanton { get; set; }

        [ForeignKey("idProvincia")]
        public TProvincia TProvincia { get; set; }


        public long Cedula { get; set; }

        //public TDistrito Distrito { get; set; }

        //[ForeignKey("Cedula")]

        public TClientes clientes { get; set; }
    }
}
