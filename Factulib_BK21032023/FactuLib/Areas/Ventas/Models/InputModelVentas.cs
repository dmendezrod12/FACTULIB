using System.ComponentModel.DataAnnotations;
using System;

namespace FactuLib.Areas.Ventas.Models
{
    public class InputModelVentas : TTemporalVentas
    {
        [Required(ErrorMessage = "El campo Cliente es Obligatorio")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "El campo Nombre Producto es Obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo cantidad es obligarorio")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El campo precio es obligatorio")]

        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El Pago no es correcto")]
        public float Precio { get; set; }

        public float Descuento { get; set; }


        [Required(ErrorMessage = "El campo pago es obligatorio")]
        [RegularExpression(@"^\s*(?=.*[0-9])\d*(?:\.\d{1,2})?\s*$", ErrorMessage = "El pago no es correcto")]
        public float Pagos { get; set; }

        //[Required(ErrorMessage = "El campo Factura es obligatorio")]

        public bool Credito { get; set; }

        public bool Contado { get; set; }

        public bool ChkEfectivo { get; set; }

        public bool ChkTransferencia { get; set; }

        public bool ChkTarjeta { get; set; }

        public byte[] Image { get; set; }

        public DateTime horaInicio { get; set; }

        public DateTime horaFinal { get; set; }
    }
}
