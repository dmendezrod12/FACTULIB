using FactuLib.Areas.Principal.Models;
using FactuLib.Areas.Users.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
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
        public String estadoCaja;
        public LCierre _cierre;
        private static PrincipalModel models;

        public PrincipalController(ApplicationDbContext context)
        {
            _cierre = new LCierre(context);
            models = new PrincipalModel();
        }

        // GET: PrincipalController
        public ActionResult Principal()
        {
            string estado = _cierre.estadoCajaActual();
            models.estadoCaja = estado;
            return View(models);
        }

    }
}
