using FactuLib.Areas.Clientes.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FactuLib.Models;

namespace FactuLib.Areas.Proveedores.Models
{
    public class TDireccionesProveedor
    {
        [Key]
        public int idDireccion { get; set; }

        public string Direccion { get; set; }


        [ForeignKey("idDistrito")]

        public int idDistrito { get; set; }

        [ForeignKey("idProvincia")]
        public TProvincia TProvincia { get; set; }

        [ForeignKey("idCanton")]
        public TCanton TCanton { get; set; }

        [ForeignKey("IdProveedor")]

        public TProveedor proveedor { get; set; }
    }
}
