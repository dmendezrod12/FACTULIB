using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Proveedores.Models
{
    public class InputModelProveedor : TCreditoProveedor
    {
        public int idProveedor { get; set; }

        [Required(ErrorMessage = "El campo Cedula Juridica es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de cedula ingresado no es válido")]

        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "El campo nombre de Proveedor es obligatorio.")]
        public string NombreProveedor { get; set; }

        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo correo electronico no es una dirección de correo electrónico válida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo Telefono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de telefono ingresado no es válido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "El campo Distrito es obligatorio.")]
        public int Distrito { get; set; }

        [Required(ErrorMessage = "El campo Canton es obligatorio.")]
        public int Canton { get; set; }

        [Required(ErrorMessage = "El campo Provincia es obligatorio.")]

        public DateTime horaInicio { get; set; }

        public DateTime horaFinal { get; set; }

        public int Provincia { get; set; }

        public string nombreProvincia { get; set; }

        public string nombreDistrito { get; set; }

        public string nombreCanton { get; set; }

        public int IdPago { get; set; }
        public decimal Pago { get; set; }
        public decimal DeudaAnterior { get; set; }
        public string IdUser { get; set; }
        public string User { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
     
    }
}
