using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactuLib.Areas.Analisis.Controllers
{
    [Authorize]
    [Area("Analisis")]
    public class AnalisisController : Controller
    {
        public IActionResult Analisis()
        {
            return View();
        }
    }
}
