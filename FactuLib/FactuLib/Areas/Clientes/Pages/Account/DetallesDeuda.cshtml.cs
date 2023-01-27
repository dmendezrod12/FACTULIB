using Azure;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Drawing.Text;
using System.Threading.Tasks;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    [Authorize]
    public class DetallesDeudaModel : PageModel
    {
        private static int _idPago = 0;
        private static int _cedula = 0;
        public string Moneda = "¢";
        private static string _errorMessage;
        public static InputModelRegistrar _dataClient;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LCliente _cliente;

        public DetallesDeudaModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _context = context;
            _userManager = userManager;
            _codigos = new LCodigos();
            _cliente = new LCliente(context);
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Moneda { get; set; } = "¢";

            public InputModelRegistrar DataClient { get; set; }


        }
        public IActionResult OnGet(int idPago, int cedula)
        {
            if (_idPago.Equals(0) && _cedula.Equals(0))
            {
                _idPago = idPago;
                _cedula = cedula;
            }
            else
            {
                if(_cedula != cedula)
                {
                    _idPago = idPago;
                    _cedula = cedula;
                    //return Redirect("/Clientes/CreditoCliente?id="+cedula +"&area=Clientes");
                }
            }
            _dataClient = _cliente.getTClientesPagos(idPago);
            Input = new InputModel
            {
                DataClient = _dataClient
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var nombre = $"{_dataClient.Name} {_dataClient.Apellido1} {_dataClient.Apellido2}";
            var deuda = String.Format("{0:#,###,###,##0.00####}", _dataClient.Deuda);
            var deudaActual = String.Format("{0:#,###,###,##0,##0.00####}", _dataClient.DeudaActual);
            var pago =  String.Format("{0:#,###,###,##0.00####}", _dataClient.Pago);
            var cambio = String.Format("{0:#,###,###,##0.00####}", _dataClient.Cambio);
            var mensual = String.Format("{0:#,###,###,##0,##0.00####}", _dataClient.Mensual);
            //var deudaAnterior = String.Format("{0:#,###,###,##0,##0.00####}", _dataClient.deudaAnterior);

            LTicket TicketPago = new LTicket();
            //TicketPago.AbreCajon();
            TicketPago.TextoCentro("Libreria y Bazar Dieka");
            TicketPago.TextoIzquierda("Dirección");
            TicketPago.TextoIzquierda("La Trinidad de Moravia");
            TicketPago.TextoIzquierda("Telefono: 22299553");
            TicketPago.LineasGuion();
            TicketPago.TextoCentro("Recibo de Pago");
            TicketPago.LineasGuion();
            TicketPago.TextoIzquierda($"Recibo: {_dataClient.Ticket}");
            TicketPago.TextoIzquierda($"Cliente: {nombre}");
            TicketPago.TextoIzquierda($"Fecha: {_dataClient.FechaPago.ToString("dd/MMM/yyy")}");
            TicketPago.TextoIzquierda($"Usuario: {_dataClient.User}");
            TicketPago.LineasGuion();
            TicketPago.TextoCentro($"Total inicial credito {Moneda}{deuda}");
            TicketPago.TextoExtremo("Cuotas por mes:", $"{Moneda}{mensual}");
            //TicketPago.TextoExtremo("Deuda anterior:", $"{Moneda}{deudaAnterior}");
            TicketPago.TextoExtremo("Pago:", $"{Moneda}.{pago}");
            TicketPago.TextoExtremo("Cambio:", $"{Moneda}{cambio}");
            TicketPago.TextoExtremo("Deuda actual:", $"{Moneda}{deudaActual}");
            TicketPago.TextoExtremo("Proximo pago:", $"{_dataClient.FechaLimite.ToString("dd/MMM/yyy")}");
            TicketPago.TextoCentro("Muchas gracias");
            //TicketPago.CortaTicket();
            TicketPago.ImprimirTicket("Microsoft XPS Document Writer");
            return Redirect("/Clientes/CreditoCliente?id=" + _cedula +"&area=Clientes");
        }
    }
}
