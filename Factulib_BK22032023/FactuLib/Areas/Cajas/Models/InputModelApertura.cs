using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Cajas.Models
{
    public class InputModelApertura : TRegistroApertura
    {
        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]

        public int CantMonedasCinco { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantMonedasDiez { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantMonedasVeinticinco { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantMonedasCincuenta { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantMonedasCien { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantMonedasQuinientos { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBilletesMil { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBilletesDosMil { get; set;} = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBilletesCincoMil { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBilletesDiezMil { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBillesVeinteMil { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]
        public int CantBilletesCincuentaMil { get; set; } = 0;

        [Required(ErrorMessage = "El campo de cantidad de dinero es obligatorio.")]

        public float CantDineroCuentas { get; set; } = 0;

        public string ErrorMessage { get; set; }


    }
}
