using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FactuLib.Areas.Compras.Pages.Account
{
    public class ListaProductosModel : PageModel
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

        public ListaProductosModel(
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
                Lista_Productos = _compras.GetProductosLista(id, 5, search, Request),
            };
        }


        public async Task<IActionResult> OnPost(int SelProd, int IdProd)
        {
            if (SelProd == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Compras/AgregarCompra?area=Compras&idProducto=" + IdProd);


            }
            else
            {
                return Redirect("/Compras/ListaClientes");
            }
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelProductos
        {
            public DataPaginador<TProducto> Lista_Productos { get; set; }

        }
    }
}
