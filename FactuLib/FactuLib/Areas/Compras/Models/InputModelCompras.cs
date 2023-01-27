using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Compras.Models
{
    public class InputModelCompras : TTemporalCompras
    {
        [Required(ErrorMessage = "El campo pago es obligatorio")]
        [RegularExpression(@"^\s*(?=.*[0-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto")]
        public float Pagos { get; set; }

        public bool Credito { get; set; }
    }
}
