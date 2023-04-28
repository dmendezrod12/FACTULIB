using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace FactuLib.Areas.Ventas.Pages.Account
{
    public class DetallesVentaModel : PageModel
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

        public DetallesVentaModel(
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
        public void OnGet(int idVenta, int id)
        {
            var clientes = _context.TClientes.ToList();
            var datosRegistrosVentas = _context.TRegistroVentas.Where(r => r.Id_RegistroVentas.Equals(idVenta)).ToList().Last();
            Input = new InputModel
            {
                IdRegistroVenta = datosRegistrosVentas.Id_RegistroVentas,
                FechaVenta = datosRegistrosVentas.Fecha_Compra,
                NombreCliente = _cliente.getNombreCliente(datosRegistrosVentas.TClientes.IdCliente),
                MetodoPago = datosRegistrosVentas.MetodoPago,
                TotalDescuentos = datosRegistrosVentas.Total_Descuento,
                TotalImpuestos = datosRegistrosVentas.Total_IVA,
                TotalNeto = datosRegistrosVentas.Total_Neto,
                TDetallesVentas = _ventas.GetDetallesVentas(datosRegistrosVentas.Id_RegistroVentas, id, 5, Request)
            };
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelVentas
        {
            public DataPaginador<TDetallesVentas> TDetallesVentas { get; set; }
            public string NombreCliente { get; set; }

            public string ErrorMessage { get; set; }

            public LCliente cliente { get; set; }

            public int IdRegistroVenta { get; set; }

            public int MetodoPago { get; set; }

            public DateTime FechaVenta { get; set; }
        }
    }
}
