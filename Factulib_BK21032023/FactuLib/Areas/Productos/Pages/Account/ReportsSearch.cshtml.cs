using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactuLib.Areas.Productos.Pages.Account
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
            lst.Add(new SelectListItem() { Text = "Reporte total de Inventario", Value = "1" });

            Input = new inputModel
            {
                opcion = lst
            };

        }

        public async Task<IActionResult> OnPost()
        {
            _dataInput = Input;
            if (_dataInput.Opcion == "Reporte total de Inventario")
            {
                return Redirect("/Productos/ReportesProductos?area=Productos");
            }
            else
            {
                return Redirect("/Productos/Productos?area=Productos");
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
