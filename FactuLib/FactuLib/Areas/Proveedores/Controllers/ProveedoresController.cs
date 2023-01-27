using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FactuLib.Areas.Proveedores.Controllers
{
    [Authorize]
    [Area("Proveedores")]
    public class ProveedoresController : Controller
    {
        private LProveedor _proveedor;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelProveedor> models;

        public ProveedoresController(
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context
            )
        {
            _signInManager = signInManager;
            _proveedor = new LProveedor( context );
        }
        public IActionResult Proveedores(int id, String filtrar)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _proveedor.getProvedores(filtrar, 0);
                if (data.Count > 0)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelProveedor>().paginador(data, id, 10, "Proveedores", "Proveedores", "Proveedores", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelProveedor>();
                }
                models = new DataPaginador<InputModelProveedor>
                {
                    List = (List<InputModelProveedor>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelProveedor()
                };
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}
