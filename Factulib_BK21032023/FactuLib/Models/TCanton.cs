using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Models
{
    public class TCanton
    {
        [System.ComponentModel.DataAnnotations.Key]

        public int idCanton { get; set; }

        public string nombreCanton { get; set; }

        [ForeignKey("idProvincia")]
        public TProvincia provincia { get; set; }
    }
}
