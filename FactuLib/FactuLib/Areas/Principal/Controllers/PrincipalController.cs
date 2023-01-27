using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Principal.Controllers
{
    [Area("Principal")]
    public class PrincipalController : Controller
    {
        // GET: PrincipalController
        public ActionResult Principal()
        {
            return View();
        }

    }
}
