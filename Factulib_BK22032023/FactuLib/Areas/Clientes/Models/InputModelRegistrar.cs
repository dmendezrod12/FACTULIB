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
        //Propiedad que almacena el Id de Cliente
        public int idCliente { get; set; }
        
        //Propiedad que almacena el numero de cedula Nacional, este es un valor numero de nueve digitos según se observa en la expresion regular
        [Required(ErrorMessage = "El campo Cedula Nacional es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de cedula nacional ingresado no es válido")]
        public long Cedula { get; set; }

        //Propiedad que almacena el numero de cedula en caso de una actualización de un registro.
        public long CedulaActulizar { get; set; }

        //Propiedad que almacena el numero de cedula juridica, este es un valor numero de diez digitos según se observa en la expresion regular
        [Required(ErrorMessage = "El campo Cedula Juridica es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{5})$", ErrorMessage = "El formato de cedula Juridica ingresado no es válido")]
        public long CedulaJuridica { get; set; }

        //Propiedad que almacena el numero de cedula DIMEX, este es un valor numero de doce digitos según se observa en la expresion regular
        [Required(ErrorMessage = "El campo documento identificación Dimex es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{6})$", ErrorMessage = "El formato de documento de identificación DIMEX ingresado no es válido")]
        public long CedulaResidencia { get; set; }

        //Propiedad que almacena el numero de cedula NITE, este es un valor numero de diez digitos según se observa en la expresion regular

        [Required(ErrorMessage = "El campo documento identificación NITE es obligatorio.")]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{5})$", ErrorMessage = "El formato de documento de identificación NITE ingresado no es válido")]
        public long CedulaNITE { get; set; }

        //Propiedad que almacena el nombre del cliente este campo es obligatorio

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Name { get; set; }

        //Propiedad que almacena el primer apellido del cliente este campo es obligatorio
        [Required(ErrorMessage = "El campo de Primer Apellido es obligatorio.")]
        public string Apellido1 { get; set; }

        //Propiedad que almacena el segundo apellido del cliente este campo es obligatorio
        [Required(ErrorMessage = "El campo de Segundo Apellido es obligatorio.")]
        public string Apellido2 { get; set; }

        //Propiedad que almacena el correo del cliente este campo es obligatorio tiene una validación de formato de correo electronico
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo correo electronico no es una dirección de correo electrónico válida")]
        public string Email { get; set; }

        //Propiedad que almacena el nombre del cliente este campo es obligatorio
        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        public string Direction { get; set; }

        //Propiedad que almacena el telefono del cliente este campo es obligatorio
        [Required(ErrorMessage = "El campo Telefono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{4})$", ErrorMessage = "El formato de telefono ingresado no es válido")]
        public string Phone { get; set; }

        //Propiedad que almacena la fecha de registro al sistema del cliente este campo es obligatorio, sin emabargo el mismo se asigna en
        //el proceso con la fecha actual del sistema

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //Propiedad que identifica si el cliente acepta crédito o no

        public bool Credit { get; set; }

        //Propiedad que almacena la imagen del registro

        public byte[] Image { get; set; }

        //Propiedad que almacena el nombre de Distrito seleccionado

        public string nombreDistrito { get; set; }

        //Propiedad que almacena el nombre de Canton seleccionado

        public string nombreCanton { get; set; }

        //Propiedad que almacena el nombre de provincia seleccionado

        public string nombreProvincia { get; set; }

        //Propiedad que almacena la dirección exacta del cliente

        public string direccionExacta { get; set; }

        public DateTime horaInicio { get; set; }

        public DateTime horaFinal { get; set; }

        public int IdPago { get; set; }

        public Decimal Pago { get; set; }

        public String IdUser { get; set; }

        public String User { get; set; }

        public long CedulaDelete { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
}
