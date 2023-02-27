using FactuLib.Areas.Cajas.Models;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Users.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactuLib.Areas.Cajas.Pages.Account
{
    public class Cierre_CajaModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private ApplicationDbContext _context, _context2;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LApertura _apertura;
        private LCierre _cierre;
        private LCompras _compras;
        private LVentas _ventas;
        private static InputModelCierre _modelCierre;
        public string Moneda = "¢";

        public Cierre_CajaModel(
        ApplicationDbContext context,
        ApplicationDbContext context2,
        IWebHostEnvironment environment,
        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _context2 = context2;
            _environment = environment;
            _userManager = userManager;
            _apertura = new LApertura(context);
            _cierre = new LCierre(context);
            _ventas = new LVentas(context);
            _compras = new LCompras(context);
            Moneda = "¢";
        }
        public void OnGet(bool error)
        {
            string idUser = _userManager.GetUserId(User);
            Input = new InputModel
            {
                CantMonedasCinco = 0,
                CantMonedasDiez = 0,
                CantMonedasVeinticinco = 0,
                CantMonedasCincuenta = 0,
                CantMonedasCien = 0,
                CantMonedasQuinientos = 0,
                CantBilletesMil = 0,
                CantBilletesDosMil = 0,
                CantBilletesCincoMil = 0,
                CantBilletesDiezMil = 0,
                CantBillesVeinteMil = 0,
                CantBilletesCincuentaMil = 0
            };

            Input.TotalVentas = _ventas.getTotalVentasCajasCierre(idUser);
            Input.TotalVentasCuentasBancarias = _ventas.getTotalVentasCuentasCierre(idUser);
            Input.TotalVentasCredito = _ventas.getTotalVentasCreditoCierre(idUser);
            Input.TotalCompras = _compras.getTotalComprasCajasCierre(idUser);
            Input.TotalComprasCuentasBancarias = _compras.getTotalComprasCuentasCierre(idUser);
            Input.TotalComprasCredito = _compras.getTotalComprasCreditoCierre(idUser);
            Input.MontoCajasApertura = _apertura.getTotalCajasApertura(idUser);
            Input.MontoCuentasApertura = _apertura.getTotalCuentasApertura(idUser);

            if (error == true)
            {
                Input.ErrorMessage = _dataInput.ErrorMessage;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (await SaveAsync())
            {
                return Redirect("/Cajas/Cierre_Caja?area=Cajas");
            }
            else
            {
                return Redirect("/Cajas/Cierre_Caja?area=Cajas&error=true");
            }
        }


        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelCierre
        {
            public string Moneda = "¢";

        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var result = false;
            var usuario = _context2.TUsers.Where(u => u.IdUser.Equals(_userManager.GetUserId(User))).ToList().Last();
            var ventas = _context2.TRegistroVentas.Where(v => v.Estado_Venta == true && v.TUser.IdUser.Equals(usuario.IdUser)).ToList();
            var compras = _context2.TRegistroCompras.Where(c => c.Estado_Compra == true && c.TUser.IdUser.Equals(usuario.IdUser)).ToList();
            var apertura = _context2.TRegisroApertura.Where(a => a.Estado == true && a.TUser.IdUser.Equals(usuario.IdUser)).ToList();
            var strategy = _context2.Database.CreateExecutionStrategy();

            _dataInput.TotalVentas = _ventas.getTotalVentasCajasCierre(usuario.IdUser);
            _dataInput.TotalVentasCuentasBancarias = _ventas.getTotalVentasCuentasCierre(usuario.IdUser);
            _dataInput.TotalVentasCredito = _ventas.getTotalVentasCreditoCierre(usuario.IdUser);
            _dataInput.TotalCompras = _compras.getTotalComprasCajasCierre(usuario.IdUser);
            _dataInput.TotalComprasCuentasBancarias = _compras.getTotalComprasCuentasCierre(usuario.IdUser);
            _dataInput.TotalComprasCredito = _compras.getTotalComprasCreditoCierre(usuario.IdUser);
            _dataInput.MontoCajasApertura = _apertura.getTotalCajasApertura(usuario.IdUser);
            _dataInput.MontoCuentasApertura = _apertura.getTotalCuentasApertura(usuario.IdUser);

            if (!_apertura.ValidaApertura(usuario.IdUser))
            {
                if (ModelState.IsValid)
                {
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var montoCajas = (_dataInput.CantMonedasCinco * 5) + (_dataInput.CantMonedasDiez * 10) + (_dataInput.CantMonedasVeinticinco * 25) + (_dataInput.CantMonedasCincuenta * 50) +
                                    (_dataInput.CantMonedasCien * 100) + (_dataInput.CantMonedasQuinientos * 500) + (_dataInput.CantBilletesMil * 1000) + (_dataInput.CantBilletesDosMil * 2000)
                                    + (_dataInput.CantBilletesCincoMil * 5000) + (_dataInput.CantBilletesDiezMil * 10000) + (_dataInput.CantBillesVeinteMil * 20000) +
                                    (_dataInput.CantBilletesCincuentaMil * 50000);

                                var cierre = new TRegistroCierre()
                                {
                                    TUser = usuario,
                                    TRegistroApertura = apertura.Last(),
                                    Fecha_Cierre = DateTime.Now,
                                    Dinero_Cajas = montoCajas,
                                    Dinero_Cuentas = _dataInput.Dinero_Cuentas,
                                    Total_Ventas_Efectivo = _dataInput.TotalVentas,
                                    Total_Ventas_Cuentas = _dataInput.TotalVentasCuentasBancarias,
                                    Total_Ventas_Credito = _dataInput.TotalVentasCredito,
                                    Total_Compras_Efectivo = _dataInput.TotalCompras,
                                    Total_Compras_Credito = _dataInput.TotalComprasCredito,
                                    Total_Compras_Cuentas = _dataInput.TotalComprasCuentasBancarias,
                                    Monto_Faltante = 0,
                                };
                                await _context.AddAsync(cierre);
                                _context.SaveChanges();

                                foreach (var item in ventas)
                                {
                                    item.Estado_Venta = false;
                                    _context.Update(item);
                                    _context.SaveChanges();
                                }

                                foreach (var item in compras)
                                {
                                    item.Estado_Compra = false;
                                    _context.Update(item);
                                    _context.SaveChanges();
                                }

                                foreach (var item in apertura)
                                {
                                    item.Estado = false;
                                    _context.Update(item);
                                    _context.SaveChanges();
                                }
                                transaction.Commit();
                                _dataInput = null;
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                                result = false;
                            }
                        }
                    });
                }
                else
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            _dataInput.ErrorMessage += error.ErrorMessage;
                        }
                    }
                    result = false;
                }
            }
            else
            {
                _dataInput.ErrorMessage = "No hay cajas aperturadas para realizar la operación";
                result = false;
            }


            return result;
        }

    }
}
