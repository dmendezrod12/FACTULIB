using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;


namespace FactuLib.Areas.Clientes.Controllers
{
    [Authorize]
    [Area("Clientes")]
    public class ClientesController : Controller
    {
        private LCliente _cliente;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegistrar> models;
        private ApplicationDbContext _context;

        public ClientesController(
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _cliente = new LCliente(context);
        }
        public IActionResult Clientes(int id, String filtrar)
        {

            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _cliente.getTClienteAsync(filtrar, 0);
                if (data.Count > 0)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegistrar>().paginador(data, id, 10, "Clientes", "Clientes", "Clientes", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelRegistrar>();
                }
                models = new DataPaginador<InputModelRegistrar>
                {
                    List = (List<InputModelRegistrar>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegistrar()
                };
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult ReportesClientes(String opcion)
        {
            Object[] objects = new Object[3];
            // Realiza la consulta de clientes y lo almacena en una variable 
            var data = _cliente.getTClienteAsync(null, 0);
            var url = Request.Scheme + "://" + Request.Host.Value;
            objects = new LPaginador<InputModelRegistrar>().paginador(data, 0, 10, "Clientes", "Clientes", "Reportes", url);
            // Define la URL de la Cabecera 
            string _headerUrl = Url.Action("HeaderPDF", "Clientes", null, "https");
            // Define la URL del Pie de página
            string _footerUrl = Url.Action("FooterPDF", "Clientes", null, "https");

            // Define la clase de modelo para enviar a la vista HTML que se convertira en PDF 
            models = new DataPaginador<InputModelRegistrar>
            {
                List = (List<InputModelRegistrar>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelRegistrar()
            };

            //Genera PDF a partir de la clase ViewAsPDF de la librería de Rotativa
            return new ViewAsPdf("Reports/ReportesClientes", models)
            {
                //..
            };
        }

        public ActionResult ReportesPagosClientes(int Cedula)
        {
            Object[] objects = new Object[3];
            var data = _cliente.getTClienteAsync(null, 0);
            var url = Request.Scheme + "://" + Request.Host.Value;
            objects = new LPaginador<InputModelRegistrar>().paginador(data, 0, 10, "Clientes", "Clientes", "Reportes", url);
            // Define la URL de la Cabecera 
            string _headerUrl = Url.Action("HeaderPDF", "Clientes", null, "https");
            // Define la URL del Pie de página
            string _footerUrl = Url.Action("FooterPDF", "Clientes", null, "https");

            InputModelRegistrar input = new InputModelRegistrar();

            DataPaginador<THistoricoPagosClientes>  models = _cliente.GetPagosReporteCliente(Cedula, 0, 10, input, Request);

            return new ViewAsPdf("Reports/ReportesPagosClientes", models)
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

        public JsonResult GetCantonesList(int idProvincia)
        {
            using (_context = new ApplicationDbContext())
            {
                var cantones = _context.TCanton.Where(x => x.provincia.idProvincia.Equals(idProvincia)).ToList();

                return Json(cantones);
            }
        }

        public JsonResult GetDistritosList(int idCanton)
        {
            using (_context = new ApplicationDbContext())
            {
                var distritos = _context.TDistrito.Where(x => x.canton.idCanton.Equals(idCanton)).ToList();

                return Json(distritos);
            }
        }
    }
}
