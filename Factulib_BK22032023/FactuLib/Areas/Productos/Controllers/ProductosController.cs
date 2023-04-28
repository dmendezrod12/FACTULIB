using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;

namespace FactuLib.Areas.Productos.Controllers
{
    [Authorize]
    [Area("Productos")]
    public class ProductosController : Controller
    {
        private LProducto _producto;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelProductos> models;
        private ApplicationDbContext _context;

        public ProductosController(
           SignInManager<IdentityUser> signInManager,
           ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _producto = new LProducto(context);
        }

        public IActionResult Productos(int id, String filtrar)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _producto.getProductosAsync(filtrar, 0);
                if (data.Count > 0)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelProductos>().paginador(data, id, 10, "Productos", "Productos", "Productos", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelProductos>();
                }
                models = new DataPaginador<InputModelProductos>
                {
                    List = (List<InputModelProductos>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelProductos()
                };
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult ReportesProductos(String opcion)
        {
            Object[] objects = new Object[3];
            var data = _producto.getProductosAsync(null, 0);
            var url = Request.Scheme + "://" + Request.Host.Value;
            objects = new LPaginador<InputModelProductos>().paginador(data, 0, 10, "Productos", "Productos", "Reportes", url);
            // Define la URL de la Cabecera 
            string _headerUrl = Url.Action("HeaderPDF", "Clientes", null, "https");
            // Define la URL del Pie de página
            string _footerUrl = Url.Action("FooterPDF", "Clientes", null, "https");

            models = new DataPaginador<InputModelProductos>
            {
                List = (List<InputModelProductos>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelProductos()
            };

            return new ViewAsPdf("ReportesProductos", models)
            {
                //..
            };
        }
    }
}
