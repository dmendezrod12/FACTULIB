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
        
        private LCliente _clientes;

        public DetailsModel(
            ApplicationDbContext context)
        {
            _clientes = new LCliente(context);
        }
        public void OnGet(long id)
        {
            var data = _clientes.getTClienteAsync(null, id);
            if (data.Count > 0)
            {
                Input = new InputModel
                {
                    datosCliente = data.ToList().Last()
                };

            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public InputModelRegistrar datosCliente { get; set; }
        }

    }
}
