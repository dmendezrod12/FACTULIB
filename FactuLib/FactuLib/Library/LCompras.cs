using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactuLib.Library
{
    public class LCompras : ListObject
    {

        public LCompras (ApplicationDbContext context)
        {
            _context= context;
        }

        public DataPaginador<TTemporalCompras> Get_Temporal_Compras (int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = getTemporal_Compra(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TTemporalCompras>().paginador(data, page, num, "Compras", "Compras", "AgregarCompra", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TTemporalCompras>();
            }
            var models = new DataPaginador<TTemporalCompras>
            {
                List = (List<TTemporalCompras>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TTemporalCompras()
            };
            return models;
        }

        public DataPaginador<TProveedor> GetProveedoreslista(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetProveedores(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TProveedor>().paginador(data, page, num, "Compras", "Compras", "AgregarCompra", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TProveedor>();
            }
            var models = new DataPaginador<TProveedor>
            {
                List = (List<TProveedor>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TProveedor()
            };
            return models;
        }

        public List<TProveedor> GetProveedores(String valor)
        {
            List<TProveedor> listTemporal;
            var TemporalLista = new List<TProveedor>();
            if (valor == null)
            {
                listTemporal = _context.TProveedor.Where(p=> p.Enabled == true).ToList();
            }
            else
            {
                listTemporal = _context.TProveedor.Where(c => c.Nombre_Proveedor.StartsWith(valor) || c.Ced_Jur.Equals(valor) && c.Enabled == true).ToList();
            }
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    TemporalLista.Add(new TProveedor
                    {
                        Nombre_Proveedor = item.Nombre_Proveedor,
                        Ced_Jur= item.Ced_Jur,
                    });
                }
            }

            return TemporalLista;
        }

        public DataPaginador<TProducto> GetProductosLista(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetProductos(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TProducto>().paginador(data, page, num, "Compras", "Compras", "AgregarCompra", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TProducto>();
            }
            var models = new DataPaginador<TProducto>
            {
                List = (List<TProducto>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TProducto()
            };
            return models;
        }

        public List<TProducto> GetProductos(String valor)
        {
            List<TProducto> listTemporal;
            var TemporalLista = new List<TProducto>();
            if (valor == null)
            {
                listTemporal = _context.TProducto.ToList();
            }
            else
            {
                listTemporal = _context.TProducto.Where(c => c.Nombre_Producto.StartsWith(valor) || c.Codigo_Producto.Equals(valor)).ToList();
            }
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    TemporalLista.Add(new TProducto
                    {
                        Id_Producto = item.Id_Producto,
                        Codigo_Producto = item.Codigo_Producto,
                        Nombre_Producto = item.Nombre_Producto,
                        Cantidad_Producto = item.Cantidad_Producto,
                        Precio_Venta = item.Precio_Venta,
                        Descuento_Producto = item.Descuento_Producto,
                        Fecha = item.Fecha,
                        TTipoProducto= item.TTipoProducto,
                        Imagen = item.Imagen,
                        Descripcion_Producto = item.Descripcion_Producto,
                        Precio_Costo = item.Precio_Costo,
                    });
                }
            }

            return TemporalLista;
        }


        public List<TTemporalCompras> getTemporal_Compra (String valor)
        {
            List<TTemporalCompras> listTemporal;
            var TemporalLista = new List<TTemporalCompras>();
            if (valor == null)
            {
                listTemporal = _context.TTemporalCompras.ToList();
            }
            else 
            {
                listTemporal = _context.TTemporalCompras.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor)).ToList();
            }
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    TemporalLista.Add(new TTemporalCompras
                    {
                        idTempCompras = item.idTempCompras,
                        TProducto = item.TProducto,
                        Nombre = item.Nombre,
                        Descripcion = item.Descripcion,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Tproveedor = item.Tproveedor,
                        Image = item.Image,
                        TotalBruto = item.TotalBruto,
                        TotalNeto = item.TotalNeto,
                        TotalDescuentos = item.TotalDescuentos,
                        TotalImpuestos = item.TotalImpuestos,
                        TUser = item.TUser,
                        Date = item.Date,
                    });
                }
            }

            return TemporalLista;
        }

        public TTemporalCompras getCompras (int id)
        {
            var TemporalList = new TTemporalCompras();
            var proveedores = _context.TProveedor.ToList();
            var usuarios = _context.TUsers.ToList();
            var listTemporal = _context.TTemporalCompras.Where(c => c.idTempCompras.Equals(id)).ToList();
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    //var producto = _context.TProducto.Where(p => p.Id_Producto.Equals(item.TProducto.Id_Producto)).ToList().Last();
                    var proveedor = _context.TProveedor.Where(p => p.IdProveedor.Equals(item.Tproveedor.IdProveedor)).ToList().Last();
                    var user = _context.TUsers.Where(u => u.IdUser.Equals(item.TUser.IdUser)).ToList().Last();
                    TemporalList = new TTemporalCompras
                    {
                        idTempCompras = item.idTempCompras,
                        Nombre = item.Nombre,
                        Descripcion = item.Descripcion,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Tproveedor = proveedor,
                        Image = item.Image,
                        TotalBruto = item.TotalBruto,
                        TotalNeto = item.TotalNeto,
                        TotalDescuentos= item.TotalDescuentos,
                        TotalImpuestos = item.TotalImpuestos,
                        //TProducto = producto,
                        TUser = user,
                        Date = item.Date,
                    };
                }
            }
            return TemporalList;
        }

        public float getMontoTotal()
        {
            float monto = 0;
            var listTemporal = _context.TTemporalCompras.ToList();
            if (0 < listTemporal.Count)
            {
                listTemporal.ForEach(item => {
                    monto = monto + item.TotalNeto;
                });
            }
            return monto;
        }
    }
}
