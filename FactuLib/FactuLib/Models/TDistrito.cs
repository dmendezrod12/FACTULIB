using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Models
{
    public class TDistrito
    {
        [System.ComponentModel.DataAnnotations.Key]

        public int idDistrito { get; set; }

        public string nombreDistrito { get; set; }

        [ForeignKey("idCanton")]
        public TCanton canton { get; set; }

        [ForeignKey("idProvincia")]
        public TProvincia provincia { get; set; }
    }
}
