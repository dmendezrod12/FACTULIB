using FactuLib.Areas.Cajas.Models;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Cajas.Pages.Account
{
    public class Apertura_CajaModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LApertura _apertura;
        private static InputModelApertura _modelApertura;
        public string Moneda = "¢";

        public Apertura_CajaModel(
          ApplicationDbContext context,
          IWebHostEnvironment environment,
          UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _apertura = new LApertura(context);
            Moneda = "¢";
        }

        public void OnGet(bool error)
        {

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
            if (error == true)
            {
                Input.ErrorMessage = _dataInput.ErrorMessage;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (await SaveAsync())
            {
                return Redirect("/Ventas/AgregarVenta?area=Ventas");
            }
            else
            {
                return Redirect("/Cajas/Apertura_Caja?area=Cajas&error=true");
            }
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelApertura
        {
            public string Moneda = "¢";

        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var result = false;
            var usuario = _context.TUsers.Where(u => u.IdUser.Equals(_userManager.GetUserId(User))).ToList().Last();
            var strategy = _context.Database.CreateExecutionStrategy();
            if (_apertura.ValidaApertura(usuario.IdUser))
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

                                var apertura = new TRegistroApertura()
                                {
                                    TUser = usuario,
                                    Fecha_Apertura = DateTime.Now,
                                    Dinero_Cajas = montoCajas,
                                    Dinero_Cuentas = _dataInput.Dinero_Cuentas,
                                    Estado = true
                                };
                                await _context.AddAsync(apertura);
                                _context.SaveChanges();
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
                _dataInput.ErrorMessage = "Ya se realizo previamente una apertura de caja";
                result = false;
            }


            return result;
        }
    }
}
