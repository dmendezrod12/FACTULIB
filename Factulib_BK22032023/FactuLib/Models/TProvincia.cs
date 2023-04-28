using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Models
{
    public class TProvincia
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int idProvincia { get; set; }

        public string nombreProvincia { get; set; }

    }
}
