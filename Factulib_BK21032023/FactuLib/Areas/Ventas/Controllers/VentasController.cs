using Microsoft.AspNetCore.Mvc;

namespace FactuLib.Areas.Ventas.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult Ventas()
        {
            return View();
        }
    }
}
