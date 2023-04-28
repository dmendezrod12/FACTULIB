using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    public class ReporteBusquedaProveedorModel : PageModel
    {
        public static List<InputModelProveedor> _dataProveedor;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LProveedor _proveedor;

        public ReporteBusquedaProveedorModel(
           UserManager<IdentityUser> userManager,
           ApplicationDbContext context
           )
        {
            _context = context;
            _userManager = userManager;
            _proveedor = new LProveedor(context);
        }
        public IActionResult OnGet()
        {
            _dataProveedor = _proveedor.getProvedores(null, 0);

            Input = new InputModel
            {
                proveedores = _dataProveedor
            };
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public List<InputModelProveedor> proveedores;

        }
    }
}
