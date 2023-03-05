using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        // Declara propiedad para el manejo de los metodos de la clase cliente
        private LCliente _clientes;

        // Metodo constructor para declarar propiedades
        public DetailsModel(
            ApplicationDbContext context)
        {
            _clientes = new LCliente(context);
        }
        //Metodo On Get que se ejecuta al cargar la vista
        public void OnGet(long id)
        {
            //Trae los datos del cliente según el metodo getTClientesAsync
            //de la clase de clientes y los ingresa en una variable
            var data = _clientes.getTClienteAsync(null, id);

            // Si el metodo devuelve datos carga datos al
            // formulario utilizando la clase de modelo
            if (data.Count > 0)
            {
                Input = new InputModel
                {
                    datosCliente = data.ToList().Last()
                };

            }
        }

        // Propiedad de la clase de modelo 

        [BindProperty]
        public InputModel Input { get; set; }


        //Clase de Modelo para almacenar la información
        public class InputModel
        {
            public InputModelRegistrar datosCliente { get; set; }
        }

    }
}
