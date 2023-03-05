using FactuLib.Areas.Cajas.Models;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace FactuLib.Areas.Cajas.Pages.Account
{
    public class Lista_CierresModel : PageModel
    {
        private LCierre _cierre;
        private LCliente _cliente;
        private static int idCliente = 0;
        public string Moneda = "¢";
        private static string _errorMessage;
        public static InputModel _dataCierre;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public Lista_CierresModel(
          UserManager<IdentityUser> userManager,
          ApplicationDbContext context
          )
        {
            _cliente = new LCliente(context);
            _cierre = new LCierre(context);
            _context = context;
            _userManager = userManager;
            _dataCierre = new InputModel();
        }

        public void OnGet(InputModel input, int id)
        {
            InputModelRegistrar _dataInput = new InputModelRegistrar();
            _dataInput.horaInicio = input.horaInicio;
            _dataInput.horaFinal = input.horaFinal;

            Input = new InputModel
            {
                ErrorMessage = _errorMessage,
                ErrorMessageFecha = _cliente.validacionFecha(_dataInput),
            };
            _dataCierre.horaFinal = input.horaFinal;
            _dataCierre.horaInicio = input.horaInicio;
            Input.TRegistroCierre = new DataPaginador<TRegistroCierre>();
            Input.TRegistroCierre = _cierre.GetRegistroCierres(id, 5, _dataCierre, Request);
        }

        public async Task<IActionResult> OnPost()
        {
            return Redirect("/Cajas/Lista_Cierres?fechaInicio=" + Input.horaInicio + "&fechaFinal=" + Input.horaFinal + "&buscar=true");
        }

        public InputModel Input { get; set; }

        public class InputModel : InputModelCierre
        {
            public DataPaginador<TRegistroCierre> TRegistroCierre { get; set; }
            public string NombreCliente { get; set; }

            public string ErrorMessageFecha { get; set; }

            public LCliente cliente { get; set; }
        }
    }
}
