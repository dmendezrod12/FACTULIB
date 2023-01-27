using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Users.Models
{
    public class InputModelRegister
    {
        [Required(ErrorMessage = "El campo nombre es obligatorio")]

        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Primer Apellido es obligatorio")]

        public string Apellido1 { get; set; }

        [Required(ErrorMessage = "El campo Segundo Apellido es obligatorio")]

        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "El campo NID es obligatorio")]
        public string NID { get; set; }

        [Required(ErrorMessage = "El campo telefono es obligatorio")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de telefono ingresado no es válido")]

        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "El campo correo electronico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo correo electronico no es una dirección de correo electrónico válida")]

        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]

        public string Password { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "El campo validacion de contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage ="Las contraseñas no coinciden")]

        public string Password2 { get; set; }

        [Required(ErrorMessage = "Seleccione un Rol")]

        public string Role { get; set; }

        public string ID { get; set; }

        public int Id { get; set; }

        public byte [] Image { get; set; }

        public IdentityUser IdentityUser { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
}
