using FactuLib.Areas.Cajas.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace FactuLib.Areas.Cajas.Pages.Account
{
    public class DetallesCajasModel : PageModel
    {
        private LCierre _cierre;
        private LCliente _cliente;
        private static int idCliente = 0;
        public string Moneda = "¢";
        private static string _errorMessage;
        public static InputModel _dataCierres;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public DetallesCajasModel(
          UserManager<IdentityUser> userManager,
          ApplicationDbContext context
          )
        {
            _context = context;
            _userManager = userManager;
            _cierre = new LCierre(context);
            _dataCierres = new InputModel();
        }

        public void OnGet(int idCierre)
        {
            var usuarios = _context.TUsers.ToList();
            var regisgtrosApertura = _context.TRegisroApertura.ToList(); 
            var detallesCierre = _context.TRegisroCierre.Where(r => r.Id_Cierre.Equals(idCierre)).ToList().Last();

            Input = new InputModel()
            {
                TotalVentas = detallesCierre.Total_Ventas_Efectivo,
                TotalVentasCuentasBancarias = detallesCierre.Total_Ventas_Cuentas,
                TotalComprasCuentasBancarias = detallesCierre.Total_Compras_Cuentas,
                TotalCompras = detallesCierre.Total_Compras_Efectivo,
                TotalVentasCredito = detallesCierre.Total_Ventas_Credito,
                TotalComprasCredito = detallesCierre.Total_Compras_Credito,
                MontoCajasApertura = detallesCierre.TRegistroApertura.Dinero_Cajas,
                MontoCuentasApertura = detallesCierre.TRegistroApertura.Dinero_Cuentas,
                MontoCajasCierre = detallesCierre.Dinero_Cajas,
                MontoCuentasCierre = detallesCierre.Dinero_Cuentas,
                MontoFaltanteCuentas = detallesCierre.Monto_Faltante_Cuentas,
                MontoFaltanteCajas = detallesCierre.Monto_Faltante_Cajas,
                MontoSobranteCuentas = detallesCierre.Monto_Sobrante_Cuentas,
                MontoSobranteCajas = detallesCierre.Monto_Sobrante_Cajas,
        };
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelCierre
        {
            public string Moneda = "¢";
        }
    }
}
