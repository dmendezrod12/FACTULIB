using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactuLib.Areas.Ventas.Pages.Account
{
    [Authorize]
    [Area("Ventas")]
    public class ListaClientesModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private UploadImage _uploadImage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LVentas _ventas;
        private LApertura _apertura;
        private LProducto _producto;
        private LSendMails _correo;
        private static InputModelVentas _temporalVentas;
        private static byte[] imageByte = null;
        private static bool ventaIniciada = false;
        public int _idPagina;
        public string Moneda = "¢";
        public float monto, montoDescuento, montoImpuesto, montoBruto = 0;
        public static string _clienteSeleccionado;
        public static string _nombreCliente = "";

        public ListaClientesModel(
        ApplicationDbContext context,
        IWebHostEnvironment environment,
        UserManager<IdentityUser> userManager)
        {
            _uploadImage = new UploadImage();
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _ventas = new LVentas(context);
            _apertura = new LApertura(context);
            _producto = new LProducto(context);
            _correo = new LSendMails();
            Moneda = "¢";
        }

        public void OnGet(int id, string search)
        {
            Input = new InputModel
            {
                Lista_Clientes = _ventas.GetClientesLista(id, 5, search, Request),
            };
        }

        public async Task<IActionResult> OnPost(String NombreCliente, int SelCliente, long CedulaCliente)
        {
            if (SelCliente == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Ventas/AgregarVenta?area=Ventas&nombreCliente=" + NombreCliente + "&CedulaCliente="+CedulaCliente);


            }
            else
            {
                return Redirect("/Ventas/ListaClientes");
            }
        }

            [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelVentas
        {
            public DataPaginador<TClientes> Lista_Clientes { get; set; }

        }
    }
}
