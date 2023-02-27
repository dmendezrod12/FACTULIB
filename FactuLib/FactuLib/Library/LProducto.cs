using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactuLib.Library
{
    public class LProducto:ListObject
    {
        public LProducto(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<InputModelProductos> getProductosAsync(String valor, int id)
        {
            List<TProducto> listTProductos;
            var productosList = new List<InputModelProductos>();
            var tiposProductos = _context.TTipoProducto.ToList();
            if (valor == null && id.Equals(0))
            {
                listTProductos = _context.TProducto.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listTProductos = _context.TProducto.Where(u => u.Nombre_Producto.StartsWith(valor) || u.Descripcion_Producto.StartsWith(valor)).ToList();
                }
                else
                {
                    listTProductos = _context.TProducto.Where(u => u.Codigo_Producto.Equals(id)).ToList();
                }
            }
            if (!listTProductos.Count.Equals(0))
            {
                foreach (var item in listTProductos)
                {
                    if (item.Enabled)
                    {
                        productosList.Add(new InputModelProductos
                        {
                            Id_Producto = item.Id_Producto,
                            Codigo_Producto = item.Codigo_Producto,
                            Nombre_Producto = item.Nombre_Producto,
                            Descripcion_Producto = item.Descripcion_Producto,
                            Cantidad_Producto = item.Cantidad_Producto,
                            Precio_Costo = item.Precio_Costo.ToString(),
                            Precio_Venta = item.Precio_Venta.ToString(),
                            Descuento_Producto = item.Descuento_Producto,
                            Fecha = item.Fecha,
                            Imagen = item.Imagen,
                            TTipoProducto = item.TTipoProducto
                        });
                    }
                }
            }
            return productosList;
        }

        public bool ExisteProducto(int CodigoProducto)
        {
            bool resultado = false;
            var producto = _context.TProducto.Where(c => c.Codigo_Producto.Equals(CodigoProducto)).ToList();

            if (producto.Count > 0)
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }

        public string getTipoProducto (List<InputModelProductos> data)
        {
            string nombreProducto = "";
            var tiposProductos = _context.TTipoProducto.ToList();
            var idProducto = data.ToList().Last().TTipoProducto.Id_TipoProducto;
            var tipoProducto = _context.TTipoProducto.Where(t => t.Id_TipoProducto.Equals(idProducto)).ToList().Last();
            if (tipoProducto.Id_TipoProducto != 0)
            {
                nombreProducto = tipoProducto.NombreTipo;
            }
            return nombreProducto;
        }

        public List<TProducto> getProducto(int codigoProducto)
        {
            var listTProductos = new List<TProducto>();
            using (var dbContext = new ApplicationDbContext())
            {
                listTProductos = dbContext.TProducto.Where(u => u.Codigo_Producto.Equals(codigoProducto)).ToList();
            }
            return listTProductos;
        }
    }
}
