using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
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

        public ActionResult ReportesProveedores(String opcion)
        {
            Object[] objects = new Object[3];
            var data = _proveedor.getProvedores(null, 0);
            var url = Request.Scheme + "://" + Request.Host.Value;
            objects = new LPaginador<InputModelProveedor>().paginador(data, 0, 10, "Proveedores", "Proveedores", "Proveedores", url);
            // Define la URL de la Cabecera 
            string _headerUrl = Url.Action("HeaderPDF", "Proveedores", null, "https");
            // Define la URL del Pie de página
            string _footerUrl = Url.Action("FooterPDF", "Proveedores", null, "https");

            models = new DataPaginador<InputModelProveedor>
            {
                List = (List<InputModelProveedor>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelProveedor()
            };

            return new ViewAsPdf("ReportesProveedores", models)
            {
                //..
            };
        }

        public ActionResult ReportesPagosProveedores(long Cedula)
        {
            Object[] objects = new Object[3];
            var data = _proveedor.getProvedores(null, 0);
            var url = Request.Scheme + "://" + Request.Host.Value;
            objects = new LPaginador<InputModelProveedor>().paginador(data, 0, 10, "Proveedores", "Proveedores", "Proveedores", url);
            // Define la URL de la Cabecera 
            string _headerUrl = Url.Action("HeaderPDF", "Clientes", null, "https");
            // Define la URL del Pie de página
            string _footerUrl = Url.Action("FooterPDF", "Clientes", null, "https");

            InputModelProveedor input = new InputModelProveedor();
            input.horaInicio = DateTime.Now;
            input.horaFinal = DateTime.Now;

            DataPaginador<THistoricoPagosProveedor> models = _proveedor.GetPagosListaProveedor(Cedula, 0, 10, input, Request);
            models.List = _proveedor.GetPagosProveedor(input, Cedula);

            return new ViewAsPdf("ReportesPagosProveedores", models)
            {
                //..
            };
        }

        public ActionResult HeaderPDF()
        {
            return View("Reports/HeaderPDF");
        }

        public ActionResult FooterPDF()
        {
            return View("Reports/FooterPDF");
        }
    }
}
