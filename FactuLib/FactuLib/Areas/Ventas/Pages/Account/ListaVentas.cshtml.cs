using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Cajas.Models;

namespace FactuLib.Areas.Ventas.Pages.Account
{
    public class ListaVentasModel : PageModel
    {
        private LVentas _ventas;
        private LCliente _cliente;
        private static int idCliente = 0;
        public string Moneda = "¢";
        private static string _errorMessage;
        public static InputModelVentas _dataVentas;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public ListaVentasModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _context = context;
            _userManager = userManager;
            _ventas = new LVentas(context);
            _dataVentas = new InputModelVentas();
            _cliente = new LCliente(context);
            _codigos = new LCodigos();
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
            _dataVentas.horaFinal = input.horaFinal;
            _dataVentas.horaInicio = input.horaInicio;
            Input.TRegistroVentas = new DataPaginador<TRegistroVentas>();
            Input.cliente = new LCliente(_context);
            Input.TRegistroVentas = _ventas.GetRegistroVentas(id, 5, _dataVentas, Request);
        }

        public async Task<IActionResult> OnPost()
        {
            return Redirect("/Ventas/ListaVentas?fechaInicio=" + Input.horaInicio + "&fechaFinal=" + Input.horaFinal + "&buscar=true");
        }

        public InputModel Input { get; set; }

        public class InputModel
        {
            public DataPaginador<TRegistroVentas> TRegistroVentas { get; set; }
            public string NombreCliente { get; set; }

            public string ErrorMessage { get; set; }

            public DateTime horaInicio { get; set; } = DateTime.Now.Date;

            public DateTime horaFinal { get; set; } = DateTime.Now.Date;

            public string ErrorMessageFecha { get; set; }

            public LCliente cliente { get; set; }
        }
    }
}
