using FactuLib.Areas.Users.Models;
using FactuLib.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    public class ReportsSearchModel : PageModel
    {
        List<SelectListItem> _list = new List<SelectListItem>();
        private static inputModel _dataInput;



        public void OnGet()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();
            

            //De la siguiente manera llenamos manualmente,
            //Siendo el campo Text lo que ve el usuario y
            //el campo Value lo que en realidad vale nuestro valor
            lst.Add(new SelectListItem() { Text = "Reporte total de clientes", Value = "1" });
            lst.Add(new SelectListItem() { Text = "Historico de pagos por Cliente", Value = "2" });




            Input = new inputModel
            {
                opcion = lst
            };

        }

        public async Task<IActionResult> OnPost()
        {
            _dataInput = Input;
            if (_dataInput.Opcion == "Reporte total de clientes")
            {
                return Redirect("/Clientes/ReportesClientes?area=Clientes");
            }
            if (_dataInput.Opcion == "Historico de pagos por Cliente")
            {
                return Redirect("/Clientes/ReporteBusquedaCliente");
            }
            else
            {
                return Redirect("/Users/Users?area=Users");
            }

        }

        [BindProperty]

        public inputModel Input { get; set; }
        
        public class inputModel 
        { 

            public List<SelectListItem> opcion { get; set; }

            public string Opcion { get; set; }

        }
    }
}
