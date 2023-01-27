using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FactuLib.Areas.Compras.Controllers
{
    [Authorize]
    [Area("Compras")]
    public class ComprasController : Controller
    {
        public IActionResult Compras()
        {
            return View();
        }

        public JsonResult GetProducto (int idProducto)
        {
            bool existeProducto = false;
            var context = new ApplicationDbContext();
            using (context = new ApplicationDbContext())
            {
                var productos = context.TProducto.ToList();
                foreach (var item in productos)
                {
                    if (item.Id_Producto.Equals(idProducto))
                    {
                        existeProducto = true;
                    }
                }
            }
            if (idProducto != 0 && existeProducto == true)
            {
                context = new ApplicationDbContext();
                using (context = new ApplicationDbContext())
                {
                    var productos = context.TProducto.Where(p => p.Id_Producto.Equals(idProducto)).ToList().Last();
                    return Json(productos);
                }
            }
            else
            {
                context = new ApplicationDbContext();
                using (context = new ApplicationDbContext())
                {
                    var productos = context.TProducto.ToList();
                    return Json(productos);
                }
                
            }
        }
    }
}
