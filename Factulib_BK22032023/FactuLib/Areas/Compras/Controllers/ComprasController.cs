using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
                    InputModelProductos listProductos = new InputModelProductos
                    {
                        Nombre_Producto = productos.Nombre_Producto,
                        Descripcion_Producto = productos.Descripcion_Producto,
                        PrecioProductoJS = productos.Precio_Costo,
                        Descuento_Producto = productos.Descuento_Producto,
                        Base64Imagen = "data:image/jpg;base64," + Convert.ToBase64String(productos.Imagen, 0, productos.Imagen.Length)
                    };
                    return Json(listProductos);
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
