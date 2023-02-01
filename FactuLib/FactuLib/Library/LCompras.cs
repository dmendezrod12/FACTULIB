using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FactuLib.Library
{
    public class LCompras : ListObject
    {

        public LCompras(ApplicationDbContext context)
        {
            _context = context;
        }

        public DataPaginador<InputModelCompras> Get_Temporal_Compras(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = getTemporal_Compra(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<InputModelCompras>().paginador(data, page, num, "Compras", "Compras", "AgregarCompra", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<InputModelCompras>();
            }
            var models = new DataPaginador<InputModelCompras>
            {
                List = (List<InputModelCompras>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelCompras()
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
                listTemporal = _context.TProveedor.Where(p => p.Enabled == true).ToList();
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
                        Ced_Jur = item.Ced_Jur,
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
                        TTipoProducto = item.TTipoProducto,
                        Imagen = item.Imagen,
                        Descripcion_Producto = item.Descripcion_Producto,
                        Precio_Costo = item.Precio_Costo,
                    });
                }
            }

            return TemporalLista;
        }


        public List<InputModelCompras> getTemporal_Compra(String valor)
        {
            var TemporalLista = new List<InputModelCompras>();
            if (valor == null)
            {
                using (var dbContext1 = new ApplicationDbContext())
                {
                    var query = dbContext1.TTemporalCompras.Join(dbContext1.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                    {
                        t.idTempCompras,
                        t.TotalNeto,
                        t.TotalImpuestos,
                        t.TotalDescuentos,
                        t.TotalBruto,
                        t.Tproveedor,
                        t.TProducto,
                        t.TUser,
                        t.Date,
                        t.Cantidad_Compra,
                        p.Id_Producto,
                        p.Codigo_Producto,
                        p.Nombre_Producto,
                        p.Descripcion_Producto,
                        p.Cantidad_Producto,
                        p.Precio_Costo,
                        p.Precio_Venta,
                        p.Descuento_Producto,
                        p.Imagen,
                        p.TTipoProducto

                    }).ToList();

                    if (!query.Count.Equals(0))
                    {
                        foreach (var item in query)
                        {
                            TemporalLista.Add(new InputModelCompras
                            {
                                idTempCompras = item.idTempCompras,
                                TProducto = item.TProducto,
                                Nombre = item.Nombre_Producto,
                                Descripcion = item.Descripcion_Producto,
                                Cantidad = item.Cantidad_Compra,
                                Precio = (float)item.Precio_Costo,
                                Tproveedor = item.Tproveedor,
                                Image = item.Imagen,
                                TotalBruto = item.TotalBruto,
                                TotalNeto = item.TotalNeto,
                                TotalDescuentos = item.TotalDescuentos,
                                TotalImpuestos = item.TotalImpuestos,
                                Cantidad_Compra = item.Cantidad_Compra,
                                TUser = item.TUser,
                                Date = item.Date,
                            });
                        }
                    }

                    return TemporalLista;
                }
            }
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var query = dbContext.TTemporalCompras.Join(dbContext.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                    {
                        t.idTempCompras,
                        t.TotalNeto,
                        t.TotalImpuestos,
                        t.TotalDescuentos,
                        t.TotalBruto,
                        t.Tproveedor,
                        t.TUser,
                        t.Date,
                        t.TProducto,
                        t.Cantidad_Compra,
                        p.Id_Producto,
                        p.Codigo_Producto,
                        p.Nombre_Producto,
                        p.Descripcion_Producto,
                        p.Cantidad_Producto,
                        p.Precio_Costo,
                        p.Precio_Venta,
                        p.Descuento_Producto,
                        p.Imagen,
                        p.TTipoProducto

                    }).Where(t => t.Nombre_Producto.StartsWith(valor) || t.Descripcion_Producto.StartsWith(valor)).ToList();

                    if (!query.Count.Equals(0))
                    {
                        foreach (var item in query)
                        {
                            TemporalLista.Add(new InputModelCompras
                            {
                                idTempCompras = item.idTempCompras,
                                TProducto = item.TProducto,
                                Nombre = item.Nombre_Producto,
                                Descripcion = item.Descripcion_Producto,
                                Cantidad = item.Cantidad_Compra,
                                Precio =(float)item.Precio_Costo,
                                Tproveedor = item.Tproveedor,
                                Image = item.Imagen,
                                TotalBruto = item.TotalBruto,
                                TotalNeto = item.TotalNeto,
                                TotalDescuentos = item.TotalDescuentos,
                                TotalImpuestos = item.TotalImpuestos,
                                Cantidad_Compra = item.Cantidad_Compra,
                                TUser = item.TUser,
                                Date = item.Date,
                            });
                        }
                    }

                    return TemporalLista;
                }
            }
        }

        public InputModelCompras getCompras(int id)
        {
            var TemporalList = new InputModelCompras();
            var proveedores = _context.TProveedor.ToList();
            var usuarios = _context.TUsers.ToList();
            var listTemporal = _context.TTemporalCompras.Where(c => c.idTempCompras.Equals(id)).ToList();
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var query = dbContext.TTemporalCompras.Join(dbContext.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                        {
                            t.idTempCompras,
                            t.TotalNeto,
                            t.TotalImpuestos,
                            t.TotalDescuentos,
                            t.TotalBruto,
                            t.Tproveedor,
                            t.TUser,
                            t.Date,
                            t.TProducto,
                            t.Cantidad_Compra,
                            p.Id_Producto,
                            p.Codigo_Producto,
                            p.Nombre_Producto,
                            p.Descripcion_Producto,
                            p.Cantidad_Producto,
                            p.Precio_Costo,
                            p.Precio_Venta,
                            p.Descuento_Producto,
                            p.Imagen,
                            p.TTipoProducto
                        }).Where(t => t.idTempCompras.Equals(item.idTempCompras)).ToList().Last();

                        var proveedor = _context.TProveedor.Where(p => p.IdProveedor.Equals(item.Tproveedor.IdProveedor)).ToList().Last();
                        var user = _context.TUsers.Where(u => u.IdUser.Equals(item.TUser.IdUser)).ToList().Last();
                        TemporalList = new InputModelCompras
                        {
                            idTempCompras = item.idTempCompras,
                            Nombre = query.Nombre_Producto,
                            Descripcion = query.Descripcion_Producto,
                            Cantidad = query.Cantidad_Compra,
                            Precio = (float)query.Precio_Costo,
                            Tproveedor = proveedor,
                            Image = query.Imagen,
                            TotalBruto = item.TotalBruto,
                            TotalNeto = item.TotalNeto,
                            TotalDescuentos = item.TotalDescuentos,
                            TotalImpuestos = item.TotalImpuestos,
                            //TProducto = producto,
                            TUser = user,
                            Date = item.Date,
                        };
                    }
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
                listTemporal.ForEach(item =>
                {
                    monto = monto + item.TotalNeto;
                });
            }
            return monto;
        }

        public float getMontoBruto()
        {
            float montoBruto = 0;
            var listTemporal = _context.TTemporalCompras.ToList();
            if (0 < listTemporal.Count)
            {
                listTemporal.ForEach(item =>
                {
                    montoBruto = montoBruto + item.TotalBruto;
                });
            }
            return montoBruto;
        }

        public float getMontoImpuestos()
        {
            float montoImpuesto = 0;
            var listTemporal = _context.TTemporalCompras.ToList();
            if (0 < listTemporal.Count)
            {
                listTemporal.ForEach(item =>
                {
                    montoImpuesto = montoImpuesto + item.TotalImpuestos;
                });
            }
            return montoImpuesto;
        }

        public float getMontoDescuentos()
        {
            float montoDescuento = 0;
            var listTemporal = _context.TTemporalCompras.ToList();
            if (0 < listTemporal.Count)
            {
                listTemporal.ForEach(item =>
                {
                    montoDescuento = montoDescuento + item.TotalDescuentos;
                });
            }
            return montoDescuento;
        }
    }
}
