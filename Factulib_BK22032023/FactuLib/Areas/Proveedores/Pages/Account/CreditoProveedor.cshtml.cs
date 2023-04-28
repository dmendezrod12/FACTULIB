using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    [Authorize]
    public class CreditoProveedorModel : PageModel
    {
        private LProveedor _proveedor;
        private static long idProveedor = 0;
        public string moneda = "¢";
        private static string _errorMessage;
        public static InputModelProveedor _datosProveedor;
        private LCodigos _codigos;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public CreditoProveedorModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _codigos = new LCodigos();
            _proveedor = new LProveedor(context);
            moneda = "¢";
        }

        public IActionResult OnGet(long id, InputModel input)
        {
            if (idProveedor == 0)
            {
                idProveedor = id;
            }
            else
            {
                if (idProveedor != id)
                {
                    idProveedor = 0;
                    return Redirect("/Proveedores/Proveedores?area=Proveedores");
                }
            }

            _datosProveedor = _proveedor.getDeudaProveedor(id);
            _datosProveedor.horaInicio = input.horaInicio;
            _datosProveedor.horaFinal = input.horaFinal;

            Input = new InputModel
            {
                DatosProveedor = _datosProveedor,
                ErrorMessage = _errorMessage,
                TPagos = _proveedor.GetPagosListaProveedor(id, 1, 10, _datosProveedor, Request)
            };
            //Input.horaInicio = input.horaInicio;
            //Input.horaFinal = input.horaFinal;


            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUser = _userManager.GetUserId(User);
            var dateNow = DateTime.Now;
            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList();
            var name = $"{user[0].Name} {user[0].Apellido1} {user[0].Apellido2}";
            var _nombreProveedor = $"{_datosProveedor.NombreProveedor}";
            var nowDate = $"{dateNow.Day}/{dateNow.Month}/{dateNow.Year}";
            switch (Input.RadioOptions)
            {
                case 1:
                    if (_datosProveedor.Deuda.Equals(0.0m))
                    {
                        _errorMessage = "El sistema no contiene deuda";
                    }
                    else
                    {
                        String _cambio = "";
                        Decimal _deudaActual = 0.0m, cambio;
                        if (Input.Pago >= _datosProveedor.Mensual)
                        {
                            var _ticket = _codigos.codigosTickets(_datosProveedor.Ticket);

                            if (Input.Pago.Equals(_datosProveedor.DeudaActual) || Input.Pago > _datosProveedor.DeudaActual)
                            {
                                cambio = Input.Pago - _datosProveedor.DeudaActual;
                                _cambio = String.Format("{0:#,###,###,##0,##0.00####}", cambio);
                                _errorMessage = $"Cambio para el sistema {moneda}{_cambio}";
                                _deudaActual = 0.0m;
                            }
                            else
                            {
                                cambio = Input.Pago - _datosProveedor.Mensual;
                                _cambio = String.Format("{0:#,###,###,##0,##0.00####}", cambio);
                                _errorMessage = $"Cambio para el sistema {moneda}{_cambio}";
                                _deudaActual = _datosProveedor.DeudaActual - _datosProveedor.Mensual;
                            }
                            var strategy = _context.Database.CreateExecutionStrategy();
                            await strategy.ExecuteAsync(async () =>
                            {
                                using (var transaction = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var _pago = String.Format("{0:#,###,###,##0,##0.00####}", Input.Pago);
                                        var _deuda = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.Deuda);
                                        var deudaActual = String.Format("{0:#,###,###,##0,##0.00####}", _deudaActual);
                                        var _anteriorDeudaActual = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.DeudaActual);
                                        var date = DateTime.Now.AddMonths(1);
                                        var mensual = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.Mensual);
                                        var proveedor = _context.TProveedor.Where(p => p.Ced_Jur.Equals(_datosProveedor.CedulaJuridica)).ToList().ElementAt(0);

                                        if (_deudaActual.Equals(0.0))
                                        {
                                            var credito = new TCreditoProveedor
                                            {
                                                idCredito = _datosProveedor.idCredito,
                                                Deuda = 0.0m,
                                                FechaDeuda = dateNow,
                                                Mensual = 0.0m,
                                                Cambio = 0.0m,
                                                UltimoPago = 0.0m,
                                                FechaPago = dateNow,
                                                DeudaActual = 0.0M,
                                                Ticket = "0000000000",
                                                TProveedor = proveedor
                                            };
                                            _context.Update(credito);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            var credito = new TCreditoProveedor
                                            {
                                                idCredito = _datosProveedor.idCredito,
                                                Deuda = _datosProveedor.Deuda,
                                                FechaDeuda = _datosProveedor.FechaDeuda,
                                                Mensual = _datosProveedor.Mensual,
                                                Cambio = cambio,
                                                UltimoPago = Input.Pago,
                                                FechaPago = dateNow,
                                                DeudaActual = _deudaActual,
                                                Ticket = _ticket,
                                                TProveedor = proveedor
                                            };
                                            _context.Update(credito);
                                            _context.SaveChanges();

                                        }
                                        var pagos = new THistoricoPagosProveedor
                                        {
                                            Deuda = _datosProveedor.Deuda,
                                            Cambio = cambio,
                                            Pago = Input.Pago,
                                            Fecha = dateNow,
                                            DeudaActual = _deudaActual,
                                            Ticket = _ticket,
                                            FechaLimite = date,
                                            FechaDeuda = _datosProveedor.FechaDeuda,
                                            Mensual = _datosProveedor.Mensual,
                                            DeudaAnterior = _datosProveedor.DeudaActual,
                                            IdUser = idUser,
                                            User = name,
                                            proveedor = proveedor
                                        };
                                        _context.Add(pagos);
                                        _context.SaveChanges();
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
                            var mensual = String.Format("{0:#,###,###,##0,##0.00####}", _datosProveedor.Mensual);
                            _errorMessage = $"El pago debe ser: {moneda} {mensual}";
                        }
                    }

                    break;
            }
            return Redirect("/Proveedores/Credito?id=" + idProveedor);
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Moneda { get; set; }

            [Required(ErrorMessage = "Seleccione una opción")]

            public int RadioOptions { get; set; }

            [Required(ErrorMessage = "El campo de pago es obligatorio")]
            [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "El pago no es correcto")]

            public Decimal Pago { get; set; }

            public InputModelProveedor DatosProveedor { get; set; }

            [TempData]

            public string ErrorMessage { get; set; }

            // public DataPaginador<TPagos_Proveedor> TPagos { get; set; }

            public DateTime horaInicio { get; set; } = DateTime.Now;
            public DateTime horaFinal { get; set; } = DateTime.Now;

            public int AmountFees { get; set; }

            public DataPaginador<THistoricoPagosProveedor> TPagos { get; set; }


        }
    }
}
