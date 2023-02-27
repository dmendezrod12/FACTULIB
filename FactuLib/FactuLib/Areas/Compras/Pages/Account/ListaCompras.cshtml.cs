using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace FactuLib.Areas.Compras.Pages.Account
{
    public class ListaComprasModel : PageModel
    {
        private LCompras _compras;
        private LProveedor _proveedor;
        private static int idCliente = 0;
        public string Moneda = "¢";
        private static string _errorMessage;
        public static InputModelCompras _dataCompras;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public ListaComprasModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _context = context;
            _userManager = userManager;
            _compras = new LCompras(context);
            _dataCompras = new InputModelCompras();
            _proveedor = new LProveedor(context);
            _codigos = new LCodigos();
        }
        public void OnGet(InputModel input, int id)
        {
            Input = new InputModel
            {
                ErrorMessage = _errorMessage,
            };
            _dataCompras.horaFinal = input.horaFinal;
            _dataCompras.horaInicio = input.horaInicio;
            Input.TRegistroCompras = new DataPaginador<TRegistroCompras>();
            Input.proveedor = new LProveedor(_context);
            Input.TRegistroCompras = _compras.GetRegistroCompras(id, 5, _dataCompras, Request);
        }

        public async Task<IActionResult> OnPost()
        {
            return Redirect ("/Compras/ListaCompras?fechaInicio=" + Input.horaInicio + "&fechaFinal=" + Input.horaFinal + "&buscar=true");
        }

        public InputModel Input { get; set; }

        public class InputModel
        {
            public DataPaginador<TRegistroCompras> TRegistroCompras { get; set; }
            public string NombreProveedor { get; set; }

            public string ErrorMessage { get; set; }

            public DateTime horaInicio { get; set; } = DateTime.Now.Date;

            public DateTime horaFinal { get; set; } = DateTime.Now.Date;

            public string ErrorMessageFecha { get; set; }

            public LProveedor proveedor { get; set; }
        }
    }
}
