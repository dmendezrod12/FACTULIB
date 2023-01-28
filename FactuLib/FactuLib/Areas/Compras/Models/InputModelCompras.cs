using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Compras.Models
{
    public class InputModelCompras : TTemporalCompras
    {
        [Required(ErrorMessage = "El campo Proveedor es Obligatorio")]
        public string Proveedor { get; set; }

        [Required(ErrorMessage = "El campo Nombre Producto es Obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo cantidad es obligarorio")]
        public int Cantidad{ get; set; }

        [Required(ErrorMessage = "El campo precio es obligatorio")]

        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El Pago no es correcto")]
        public float Precio { get; set; }


        [Required(ErrorMessage = "El campo pago es obligatorio")]
        [RegularExpression(@"^\s*(?=.*[0-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto")]

        public float Pagos { get; set; }

        public bool Credito { get; set; }

        public bool Contado { get; set; }

        public byte[] Image { get; set; }
    }
}
