using FactuLib.Areas.Compras.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace FactuLib.Areas.Compras.Pages.Account
{
    public class DetallesCompraModel : PageModel
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

        public DetallesCompraModel(
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
        public void OnGet(int idCompra, int id)
        {
            //if (idCompra != 0)
            //{
                var proveedores = _context.TProveedor.ToList();
                var datosRegistroCompras = _context.TRegistroCompras.Where(r => r.Id_RegistroCompras.Equals(idCompra)).ToList().Last();
                Input = new InputModel
                {
                    IdRegistroCompra = datosRegistroCompras.Id_RegistroCompras,
                    FechaCompra = datosRegistroCompras.Fecha_Compra,
                    NombreProveedor = _proveedor.getNombreProveedor(datosRegistroCompras.TProveedor.IdProveedor),
                    FacturaProveedor = datosRegistroCompras.NumeroFacturaProveedor,
                    MetodoPago = datosRegistroCompras.MetodoPago,
                    TotalDescuentos = datosRegistroCompras.Total_Descuento,
                    TotalImpuestos = datosRegistroCompras.Total_IVA,
                    TotalNeto = datosRegistroCompras.Total_Neto,
                    TDetallesCompras = _compras.GetDetallesCompras(datosRegistroCompras.Id_RegistroCompras, id, 5, Request)
                };
            //} 
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelCompras
        {
            public DataPaginador<TDetallesCompras> TDetallesCompras { get; set; }
            public string NombreProveedor { get; set; }

            public string ErrorMessage { get; set; }

            public LProveedor proveedor { get; set; }

            public int IdRegistroCompra { get; set; }

            public int MetodoPago { get; set; }

            public DateTime FechaCompra { get; set; }
        }
    }
}
