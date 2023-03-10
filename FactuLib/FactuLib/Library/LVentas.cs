using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Areas.Clientes.Models;

namespace FactuLib.Library
{
    public class LVentas : ListObject
    {
        public LVentas(ApplicationDbContext context)
        {
            _context = context;
        }


        public DataPaginador<InputModelVentas> Get_Temporal_Ventas(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = getTemporal_Venta(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<InputModelVentas>().paginador(data, page, num, "Ventas", "Ventas", "AgregarVenta", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<InputModelVentas>();
            }
            var models = new DataPaginador<InputModelVentas>
            {
                List = (List<InputModelVentas>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelVentas()
            };
            return models;
        }

        public DataPaginador<TClientes> GetClientesLista(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetClientes(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TClientes>().paginador(data, page, num, "Ventas", "Ventas", "AgregarVenta", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TClientes>();
            }
            var models = new DataPaginador<TClientes>
            {
                List = (List<TClientes>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TClientes()
            };
            return models;
        }

        public List<TClientes> GetClientes(String valor)
        {
            List<TClientes> listTemporal;
            var TemporalLista = new List<TClientes>();
            if (valor == null)
            {
                listTemporal = _context.TClientes.Where(p => p.Enabled == true).ToList();
            }
            else
            {
                listTemporal = _context.TClientes.Where(c => c.Nombre.StartsWith(valor) || c.Cedula.Equals(valor) || c.Apellido1.Equals(valor) || c.Apellido2.Equals(valor) && c.Enabled == true).ToList();
            }
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    TemporalLista.Add(new TClientes
                    {
                        Nombre = item.Nombre,
                        Cedula = item.Cedula,
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
                objects = new LPaginador<TProducto>().paginador(data, page, num, "Ventas", "Ventas", "AgregarVenta", url);
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


        public List<InputModelVentas> getTemporal_Venta(String valor)
        {
            var TemporalLista = new List<InputModelVentas>();
            if (valor == null)
            {
                using (var dbContext1 = new ApplicationDbContext())
                {
                    var query = dbContext1.TTemporalVentas.Join(dbContext1.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                    {
                        t.idTempVentas,
                        t.TotalNeto,
                        t.TotalImpuestos,
                        t.TotalDescuentos,
                        t.TotalBruto,
                        t.TCliente,
                        t.TProducto,
                        t.TUser,
                        t.Date,
                        t.Cantidad_Venta,
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
                            TemporalLista.Add(new InputModelVentas
                            {
                                idTempVentas = item.idTempVentas,
                                TProducto = item.TProducto,
                                Nombre = item.Nombre_Producto,
                                Descripcion = item.Descripcion_Producto,
                                Cantidad = item.Cantidad_Venta,
                                Precio = (float)item.Precio_Costo,
                                TCliente = item.TCliente,
                                Image = item.Imagen,
                                TotalBruto = item.TotalBruto,
                                TotalNeto = item.TotalNeto,
                                TotalDescuentos = item.TotalDescuentos,
                                Descuento = item.Descuento_Producto,
                                TotalImpuestos = item.TotalImpuestos,
                                Cantidad_Venta = item.Cantidad_Venta,
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
                    var query = dbContext.TTemporalVentas.Join(dbContext.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                    {
                        t.idTempVentas,
                        t.TotalNeto,
                        t.TotalImpuestos,
                        t.TotalDescuentos,
                        t.TotalBruto,
                        t.TCliente,
                        t.TUser,
                        t.Date,
                        t.TProducto,
                        t.Cantidad_Venta,
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
                            TemporalLista.Add(new InputModelVentas
                            {
                                idTempVentas = item.idTempVentas,
                                TProducto = item.TProducto,
                                Nombre = item.Nombre_Producto,
                                Descripcion = item.Descripcion_Producto,
                                Cantidad = item.Cantidad_Venta,
                                Precio = (float)item.Precio_Costo,
                                TCliente = item.TCliente,
                                Image = item.Imagen,
                                TotalBruto = item.TotalBruto,
                                TotalNeto = item.TotalNeto,
                                TotalDescuentos = item.TotalDescuentos,
                                TotalImpuestos = item.TotalImpuestos,
                                Cantidad_Venta = item.Cantidad_Venta,
                                TUser = item.TUser,
                                Date = item.Date,
                            });
                        }
                    }
                    return TemporalLista;
                }
            }
        }

        public InputModelVentas getVentas(int id)
        {
            var TemporalList = new InputModelVentas();
            var clientes = _context.TClientes.ToList();
            var usuarios = _context.TUsers.ToList();
            var listTemporal = _context.TTemporalVentas.Where(c => c.idTempVentas.Equals(id)).ToList();
            if (!listTemporal.Count.Equals(0))
            {
                foreach (var item in listTemporal)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var query = dbContext.TTemporalVentas.Join(dbContext.TProducto, t => t.TProducto.Id_Producto, p => p.Id_Producto, (t, p) => new
                        {
                            t.idTempVentas,
                            t.TotalNeto,
                            t.TotalImpuestos,
                            t.TotalDescuentos,
                            t.TotalBruto,
                            t.TCliente,
                            t.TUser,
                            t.Date,
                            t.TProducto,
                            t.Cantidad_Venta,
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
                        }).Where(t => t.idTempVentas.Equals(item.idTempVentas)).ToList().Last();

                        var cliente = _context.TClientes.Where(p => p.IdCliente.Equals(item.TCliente.IdCliente)).ToList().Last();
                        var user = _context.TUsers.Where(u => u.IdUser.Equals(item.TUser.IdUser)).ToList().Last();
                        TemporalList = new InputModelVentas
                        {
                            idTempVentas = item.idTempVentas,
                            Nombre = query.Nombre_Producto,
                            Descripcion = query.Descripcion_Producto,
                            Cantidad = query.Cantidad_Venta,
                            Precio = (float)query.Precio_Costo,
                            TCliente = cliente,
                            Image = query.Imagen,
                            TotalBruto = item.TotalBruto,
                            TotalNeto = item.TotalNeto,
                            TotalDescuentos = item.TotalDescuentos,
                            TotalImpuestos = item.TotalImpuestos,
                            Descuento = query.Descuento_Producto,
                            //TProducto = producto,
                            TUser = user,
                            Date = item.Date,
                        };
                    }
                }
            }
            return TemporalList;
        }

        public DataPaginador<TRegistroVentas> GetRegistroVentas(int page, int num, InputModelVentas input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetRegistrosVentas(input);
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<TRegistroVentas>().paginador(data, page, 5, "Ventas", "Ventas", "ListaVentas", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TRegistroVentas>();
            }
            var models = new DataPaginador<TRegistroVentas>
            {
                List = (List<TRegistroVentas>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TRegistroVentas(),
            };
            return models;
        }


        public List<TRegistroVentas> GetRegistrosVentas(InputModelVentas input)
        {
            var listPagos = new List<TRegistroVentas>();
            var listPagos2 = new List<TRegistroVentas>();
            /*
             Menor a cero: si t1 es anterior a t2
             Cero: si t1 es lo mismo que t2
             Mayor a cero: si t1 es posterior a t2
             */
            var t1 = input.horaInicio.ToString("dd/MMM/yyy");
            var t2 = input.horaFinal.ToString("dd/MMM/yyy");
            var clientes = _context.TClientes.ToList();

            int fechaCompare = DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2));

            if (fechaCompare > 0)
            {
                listPagos2 = _context.TRegistroVentas.ToList();
            }
            else
            {
                if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
                {
                    listPagos2 = _context.TRegistroVentas.ToList();
                }
                else
                {
                    foreach (var item in _context.TRegistroVentas.ToList())
                    {
                        int fecha1 = DateTime.Compare(DateTime.Parse(item.Fecha_Compra.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                        if (fecha1.Equals(0) || fecha1 > 0)
                        {
                            listPagos.Add(item);
                        }
                    }
                    foreach (var item in listPagos)
                    {
                        int fecha2 = DateTime.Compare(DateTime.Parse(item.Fecha_Compra.ToString("dd/MMM/yyy")), DateTime.Parse(t2));
                        if (fecha2.Equals(0) || fecha2 < 0)
                        {
                            listPagos2.Add(item);
                        }
                    }
                }
            }
            return listPagos2;
        }

        public DataPaginador<TDetallesVentas> GetDetallesVentas(int id, int page, int num, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetListaDetalleVenta(id);
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<TDetallesVentas>().paginadorDetalle(id, data, page, 5, "Ventas", "Ventas", "DetallesVenta", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TDetallesVentas>();
            }
            var models = new DataPaginador<TDetallesVentas>
            {
                List = (List<TDetallesVentas>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TDetallesVentas(),
            };
            return models;
        }

        public List<TDetallesVentas> GetListaDetalleVenta(int idVenta)
        {
            var listDetalles = new List<TDetallesVentas>();
            var productos = _context.TProducto.ToList();
            var ventas = _context.TRegistroVentas.ToList();
            listDetalles = _context.TDetallesVentas.Where(d => d.TRegistroVentas.Id_RegistroVentas.Equals(idVenta)).ToList();
            return listDetalles;
        }


        public float getMontoTotal()
        {
            float monto = 0;
            var listTemporal = _context.TTemporalVentas.ToList();
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
            var listTemporal = _context.TTemporalVentas.ToList();
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
            var listTemporal = _context.TTemporalVentas.ToList();
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
            var listTemporal = _context.TTemporalVentas.ToList();
            if (0 < listTemporal.Count)
            {
                listTemporal.ForEach(item =>
                {
                    montoDescuento = montoDescuento + item.TotalDescuentos;
                });
            }
            return montoDescuento;
        }

        public float getTotalVentasCajasCierre(String IdUser)
        {
            float montoVentasCierre = 0;
            var ventasCierre = _context.TRegistroVentas.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado_Venta == true && r.MetodoPago.Equals(1)).ToList();
            if (0 < ventasCierre.Count)
            {
                ventasCierre.ForEach(item =>
                {
                    montoVentasCierre = montoVentasCierre + item.Total_Neto;
                });
            }
            return montoVentasCierre;
        }

        public float getDineroRecibidoVentas(String IdUser)
        {
            float montoDineroRecibidoVentas = 0;
            var ventasCierre = _context.TRegistroVentas.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado_Venta == true && r.MetodoPago.Equals(1)).ToList();
            if (0 < ventasCierre.Count)
            {
                ventasCierre.ForEach(item =>
                {
                    montoDineroRecibidoVentas = montoDineroRecibidoVentas + item.DineroRecibido;
                });
            }
            return montoDineroRecibidoVentas;
        }

        public float getTotalCambiosVentas(String IdUser)
        {
            float montoCambioVentas = 0;
            var ventasCierre = _context.TRegistroVentas.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado_Venta == true && r.MetodoPago.Equals(1)).ToList();
            if (0 < ventasCierre.Count)
            {
                ventasCierre.ForEach(item =>
                {
                    montoCambioVentas = montoCambioVentas + (float)item.CambioCompra;
                });
            }
            return montoCambioVentas;
        }

        public float getTotalVentasCuentasCierre(String IdUser)
        {
            float montoVentasCierre = 0;
            var ventasCierre = _context.TRegistroVentas.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado_Venta == true && (r.MetodoPago.Equals(2) || r.MetodoPago.Equals(3))).ToList();
            if (0 < ventasCierre.Count)
            {
                ventasCierre.ForEach(item =>
                {
                    montoVentasCierre = montoVentasCierre + item.Total_Neto;
                });
            }
            return montoVentasCierre;
        }

        public float getTotalVentasCreditoCierre(String IdUser)
        {
            float montoVentasCierre = 0;
            var ventasCierre = _context.TRegistroVentas.Where(r => r.TUser.IdUser.Equals(IdUser) && r.Estado_Venta == true && r.MetodoPago.Equals(0)).ToList();
            if (0 < ventasCierre.Count)
            {
                ventasCierre.ForEach(item =>
                {
                    montoVentasCierre = montoVentasCierre + item.Total_Neto;
                });
            }
            return montoVentasCierre;
        }
    }
}
