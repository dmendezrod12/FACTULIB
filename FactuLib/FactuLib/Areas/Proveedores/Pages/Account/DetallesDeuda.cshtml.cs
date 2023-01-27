using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    [Authorize]
    public class DetallesDeudaModel : PageModel
    {
        private static int _idPago = 0;
        private static long _cedula = 0;
        public string moneda = "¢";
        private static string _errorMessage;
        public static InputModelProveedor _datosProveedor;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LProveedor _proveedor; 

        public DetallesDeudaModel (
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _codigos = new LCodigos();
            _proveedor = new LProveedor(context);
        }
        public IActionResult OnGet(int idPago, long cedula)
        {
            if (_idPago.Equals(0) && _cedula.Equals(0))
            {
                _idPago = idPago;
                _cedula = cedula;
            }
            else
            {
                if (_idPago != idPago || _cedula != cedula)
                {
                    _idPago = 0;
                    return Redirect("/Proveedores/Credito?id=" + _cedula + "&area=Proveedores");
                }
            }
            _datosProveedor = _proveedor.getPagoProveedor(idPago);
            Input = new InputModel
            {
                DatosProveedor = _datosProveedor
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            var nombre = $"{_datosProveedor.NombreProveedor}";
            var deuda = String.Format("{0:#,###,###,##0.00####}", _datosProveedor.Deuda);
            var deudaActual = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.DeudaActual);
            var pago = String.Format("{0:#,###,###,##0.00####}", _datosProveedor.Pago);
            var cambio = String.Format("{0:#,###,###,##0.00####}", _datosProveedor.Cambio);
            var mensual = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.Mensual);
            var deudaAnterior = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.DeudaAnterior);

            LTicket TicketPago = new LTicket();
            //TicketPago.AbreCajon();
            TicketPago.TextoCentro("Libreria y Bazar Dieka");
            TicketPago.TextoIzquierda("Dirección");
            TicketPago.TextoIzquierda("La Trinidad de Moravia");
            TicketPago.TextoIzquierda("Telefono: 22299553");
            TicketPago.LineasGuion();
            TicketPago.TextoCentro("Comprobante de Compra");
            TicketPago.LineasGuion();
            TicketPago.TextoIzquierda($"No Comprobante: {_datosProveedor.Ticket}");
            TicketPago.TextoIzquierda($"Proveedor: {nombre}");
            TicketPago.TextoIzquierda($"Fecha: {_datosProveedor.FechaPago.ToString("dd/MMM/yyy")}");
            TicketPago.TextoIzquierda($"Usuario: {_datosProveedor.User}");
            TicketPago.LineasGuion();
            TicketPago.TextoCentro($"Total inicial credito {moneda}{deuda}");
            TicketPago.TextoExtremo("Cuotas por mes:", $"{moneda}{mensual}");
            TicketPago.TextoExtremo("Deuda anterior:", $"{moneda}{deudaAnterior}");
            TicketPago.TextoExtremo("Pago:", $"{moneda}.{pago}");
            TicketPago.TextoExtremo("Cambio:", $"{moneda}{cambio}");
            TicketPago.TextoExtremo("Deuda actual:", $"{moneda}{deudaActual}");
            TicketPago.TextoExtremo("Proximo pago:", $"{_datosProveedor.FechaLimite.ToString("dd/MMM/yyy")}");
            TicketPago.TextoCentro("Muchas gracias");
            //TicketPago.CortaTicket();
            TicketPago.ImprimirTicket("Microsoft XPS Document Writer");
            return Redirect("/Proveedores/Credito?id=" + _cedula + "&area=Clientes");
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Moneda { get; set; } = "¢";

            public InputModelProveedor DatosProveedor;

        }
    }
}
