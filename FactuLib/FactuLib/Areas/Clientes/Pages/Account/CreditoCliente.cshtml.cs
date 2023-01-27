using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    [Authorize]
    public class CreditoClienteModel : PageModel
    {
        private LCliente _cliente;
        private static int idCliente = 0;
        public  string Moneda = "¢";
        private static string _errorMessage;
        public static InputModelRegistrar _dataclient;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public CreditoClienteModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _context = context;
            _userManager = userManager;
            _cliente = new LCliente(context);
            _codigos = new LCodigos();
        }
        public IActionResult OnGet(int id, InputModel input)
        {
            idCliente = 0;
            if (idCliente == 0)
            {
                idCliente = id;
            }
            else
            {
                if (idCliente != id)
                {
                    idCliente = 0;
                    return Redirect("/Clientes/Clientes?area=Clientes");
                }
            }
            _dataclient = _cliente.getClienteCredito(id);
            _dataclient.horaInicio = input.horaInicio;
            _dataclient.horaFinal = input.horaFinal;
            Input = new InputModel
            {
                DatosCliente = _dataclient,
                ErrorMessageFecha = _cliente.validacionFecha(_dataclient),
                ErrorMessage = _errorMessage,
                TPagos = _cliente.GetPagos(id, 1, 10, _dataclient,Request)
            };
            _errorMessage = "";
            return Page();
        }

    [BindProperty]
    public InputModel Input { get; set; }

        public class InputModel
        {
            public string Moneda { get; set; } = "¢";
            [Required(ErrorMessage = "Seleccione una opción")]

            public int RadioOptions { get; set; }

            [Required(ErrorMessage = "El campo pago es obligatorio.")]
            [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "El pago no es correcto")]

            public Decimal Pago { get; set; }

            public InputModelRegistrar DatosCliente { get; set; }

            [TempData]
            public string ErrorMessage { get; set; }

            public string ErrorMessageFecha { get; set; }

            public DataPaginador<THistoricoPagosClientes> TPagos { get; set; }

            public DateTime horaInicio { get; set; } = DateTime.Now.Date;

            public DateTime horaFinal { get; set; } = DateTime.Now.Date;
        }

        
        public async Task<IActionResult> OnPost()
        {
            var idUser = _userManager.GetUserId(User);
            switch (Input.RadioOptions)
            {
                case 1:
                    if (_dataclient.Deuda.Equals(0.0m))
                    {
                        _errorMessage = "El cliente no tiene deudas activas";
                    }
                    else
                    {
                        String _cambio = "";
                        Decimal _deudaActual = 0.0m, cambio;
                        if (Input.Pago >= _dataclient.Mensual)
                        {
                            var _ticket = _codigos.codigosTickets(_dataclient.Ticket);
                            var _nombreCliente = $"{_dataclient.Name} {_dataclient.Apellido1} {_dataclient.Apellido2}";
                            var fechaActual = DateTime.Now;
                            var fechaActual2 = $"{fechaActual.Day}/{fechaActual.Month}/{fechaActual.Year}";
                            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList();
                            var name = $"{user[0].Name} {user[0].Apellido1} {user[0].Apellido1}";

                            if (Input.Pago.Equals(_dataclient.DeudaActual) || Input.Pago > _dataclient.DeudaActual)
                            {
                                cambio = Input.Pago - _dataclient.DeudaActual;
                                _cambio = String.Format("{0:#,###,###,##.0.00####}", cambio);
                                _errorMessage = $"Cambio para el cliente {Moneda}{_cambio}";
                                _deudaActual = 0.0m;
                            }
                            else
                            {
                                cambio = Input.Pago - _dataclient.Mensual;
                                _cambio = String.Format("{0:#,###,###,##.0.00####}", cambio);
                                _errorMessage = $"Cambio para el cliente {Moneda}{_cambio}";
                                _deudaActual = _dataclient.DeudaActual - _dataclient.Mensual;
                            }
                            var strategy = _context.Database.CreateExecutionStrategy();
                            await strategy.ExecuteAsync(async () => {
                                using (var transaction = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var _pago = String.Format("{0:#,###,###,##.0.00####}", Input.Pago);
                                        var _deuda = String.Format("{0:#,###,###,##.0.00####}", _dataclient.Deuda);
                                        var deudaActual = String.Format("{0:#,###,###,##.0.00####}", _deudaActual);
                                        //var cambioFinal = String.Format("{0:#,###,###,##.0.00####}", _cambio);
                                        var _deudaActualCliente = String.Format("{0:#,###,###,##.0.00####}", _dataclient.DeudaActual);
                                        var _mensual = String.Format("{0:#,###,###,##.0.00####}", _dataclient.Mensual);

                                        var date = DateTime.Now.AddMonths(1);

                                        var _fechaLimite = _dataclient.DeudaActual.Equals(0.0m) ? "--/--/--" : $"{date.Day}/{date.Month}/{date.Year}";
                                        var cliente = _context.TClientes.Where(c => c.Cedula.Equals(_dataclient.Cedula)).ToList().ElementAt(0);
                                        var credito = new TCreditoClientes
                                        {
                                            idDeuda = _dataclient.idDeuda,
                                            Deuda = _dataclient.Deuda,
                                            FechaDeuda = _dataclient.FechaDeuda,
                                            Mensual = _dataclient.Mensual,
                                            Cambio = cambio,
                                            UltimoPago = Input.Pago,
                                            FechaPago = fechaActual,
                                            DeudaActual = _deudaActual,
                                            Ticket = _ticket,
                                            FechaLimite = date,
                                            TClients = cliente
                                        };
                                        _context.Update(credito);
                                        _context.SaveChanges();

                                        var pagos = new THistoricoPagosClientes
                                        {
                                            Deuda = _dataclient.Deuda,
                                            Cambio = cambio,
                                            Pago = Input.Pago,
                                            Fecha = fechaActual,
                                            DeudaActual = _deudaActual,
                                            Ticket = _ticket,
                                            FechaLimite = date,
                                            IdUser = idUser,
                                            User = name,
                                            CedulaCliente = _dataclient.Cedula
                                        };

                                        _context.Add(pagos);
                                        _context.SaveChanges();

                                        LTicket TicketPago = new LTicket();
                                        //TicketPago.AbreCajon();
                                        TicketPago.TextoCentro("Libreria y Bazar Dieka");
                                        TicketPago.TextoIzquierda("Dirección");
                                        TicketPago.TextoIzquierda("La Trinidad de Moravia");
                                        TicketPago.TextoIzquierda("Telefono: 22299553");
                                        TicketPago.LineasGuion();
                                        TicketPago.TextoCentro("Recibo de Pago");
                                        TicketPago.LineasGuion();
                                        TicketPago.TextoIzquierda($"Recibo: {_ticket}");
                                        TicketPago.TextoIzquierda($"Cliente: {_nombreCliente}");
                                        TicketPago.TextoIzquierda($"Fecha: {fechaActual}");
                                        TicketPago.TextoIzquierda($"Usuario: {name}");
                                        TicketPago.LineasGuion();
                                        TicketPago.TextoCentro($"Total inicial credito {Moneda}{_deuda}");
                                        TicketPago.TextoExtremo("Cuotas por mes:", $"{Moneda}{_mensual}");
                                        TicketPago.TextoExtremo("Pago:", $"{Moneda}.{_pago}");
                                        TicketPago.TextoExtremo("Cambio:", $"{Moneda}{_cambio}");
                                        TicketPago.TextoExtremo("Deuda actual:", $"{Moneda}{deudaActual}");
                                        TicketPago.TextoExtremo("Proximo pago:", $"{_fechaLimite}");
                                        TicketPago.TextoCentro("Muchas gracias");
                                        //TicketPago.CortaTicket();
                                        TicketPago.ImprimirTicket("Microsoft XPS Document Writer");

                                        transaction.Commit();

                                    }
                                    catch (Exception ex)
                                    {
                                        _errorMessage = ex.Message;
                                        transaction.Rollback();
                                    }
                                }
                            });
                        }
                        else
                        {
                            var mensual = String.Format("{0:#,###,###,##0.00####", _dataclient.Mensual);
                            _errorMessage = $"El pago debe ser {Moneda}{mensual}";
                        }
                    }

                    break;
            }
            return Redirect("/Clientes/CreditoCliente?id=" + idCliente);
        }
        
    }
}
