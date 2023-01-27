using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Clientes.Models
{
    public class InputModelRegistrar : TCreditoClientes
    {
        public int idCliente { get; set; }

        [Required(ErrorMessage = "El campo Cedula Nacional es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de cedula nacional ingresado no es válido")]
        public long Cedula { get; set; }

        public long CedulaActulizar { get; set; }
        
        [Required(ErrorMessage = "El campo Cedula Juridica es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{5})$", ErrorMessage = "El formato de cedula Juridica ingresado no es válido")]
        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "El campo documento identificación Dimex es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{6})$", ErrorMessage = "El formato de documento de identificación DIMEX ingresado no es válido")]
        public long CedulaResidencia { get; set; }

        [Required(ErrorMessage = "El campo documento identificación NITE es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{5})$", ErrorMessage = "El formato de documento de identificación NITE ingresado no es válido")]
        public long CedulaNITE { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo de Primer Apellido es obligatorio.")]
        public string Apellido1 { get; set; }

        [Required(ErrorMessage = "El campo de Segundo Apellido es obligatorio.")]
        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo correo electronico no es una dirección de correo electrónico válida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        public string Direction { get; set; }

        [Required(ErrorMessage = "El campo Telefono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de telefono ingresado no es válido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        public bool Credit { get; set; }

        public byte[] Image { get; set; }

        public string nombreDistrito { get; set; }

        public string nombreCanton { get; set; }

        public string nombreProvincia { get; set; }

        public string direccionExacta { get; set; }

        public DateTime horaInicio { get; set; }

        public DateTime horaFinal { get; set; }

        public int IdPago { get; set; }

        public Decimal Pago { get; set; }

        public String IdUser { get; set; }

        public String User { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
}
