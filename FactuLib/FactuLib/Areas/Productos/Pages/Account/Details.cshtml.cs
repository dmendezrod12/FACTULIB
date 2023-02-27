using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace FactuLib.Areas.Productos.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private LProducto _producto;

        public DetailsModel(
            ApplicationDbContext context)
        {
            _producto = new LProducto(context);
        }
        public void OnGet(int id)
        {
            var data = _producto.getProductosAsync(null, id);
            var nombreTipoProducto = _producto.getTipoProducto(data);
            if (data.Count > 0)
            {
                Input = new InputModel
                {
                    datosProducto = data.ToList().Last(),
                    nombreTipo = nombreTipoProducto
                };

            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string nombreTipo { get; set; }
            public InputModelProductos datosProducto { get; set; }
        }
    }
}
