
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private LProveedor _proveedor;
        private ApplicationDbContext _context;
        public DetailsModel(
            ApplicationDbContext context) 
        {
            _proveedor = new LProveedor( context );
            _context = context;
        }
        public void OnGet(long id)
        {
            var data = _proveedor.getProvedores(null, id);
          
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DatosProveedor = data.ToList().Last(),
                };
            }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public InputModelProveedor DatosProveedor { get; set; }
        }
    }
}
