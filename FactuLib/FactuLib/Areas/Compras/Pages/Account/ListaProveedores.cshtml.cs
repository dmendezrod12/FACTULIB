using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace FactuLib.Areas.Compras.Pages.Account
{
    [Authorize]
    [Area("Compras")]
    public class ListaProveedoresModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private UploadImage _uploadImage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LCompras _compras;
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

        public ListaProveedoresModel(
        ApplicationDbContext context,
        IWebHostEnvironment environment,
        UserManager<IdentityUser> userManager)
        {
            _uploadImage = new UploadImage();
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _compras = new LCompras(context);
            _apertura = new LApertura(context);
            _producto = new LProducto(context);
            _correo = new LSendMails();
            Moneda = "¢";
        }

        public void OnGet(int id, string search)
        {
            Input = new InputModel
            {
                Lista_Proveedores = _compras.GetProveedoreslista(id, 5, search, Request),
            };
        }

        public async Task<IActionResult> OnPost(String NombreProveedor, int SelProveedor, long CedulaProveedor)
        {
            if (SelProveedor == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Compras/AgregarCompra?area=Compras&nombreProveedor=" + NombreProveedor + "&CedulaProveedor=" + CedulaProveedor);


            }
            else
            {
                return Redirect("/Ventas/ListaClientes");
            }
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelCompras
        {
            public DataPaginador<TProveedor> Lista_Proveedores { get; set; }

        }
    }
}
