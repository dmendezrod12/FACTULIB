using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactuLib.Library
{
    public class LProveedor : ListObject
    {
        public LProveedor
        (
            ApplicationDbContext context
        ) 
        { 
            _context= context;
        }

        public List<InputModelProveedor> getProvedores (string valor, long id)
        {
            List<TProveedor> listTProveedores;
            var listaProveedor = new List<InputModelProveedor>();
        
      
            var provincias = _context.TProvincia.ToList();
            var cantones = _context.TCanton.ToList();
            var distritos = _context.TDistrito.ToList();
            

            if (valor == null && id.Equals(0))
            {
                listTProveedores = _context.TProveedor.ToList();

            }
            else
            {
                if (id.Equals(0))
                {
                    listTProveedores = _context.TProveedor.Where(p => p.Nombre_Proveedor.StartsWith(valor)).ToList();

                }
                else
                {
                    listTProveedores = _context.TProveedor.Where(p=> p.Ced_Jur.Equals(id)).ToList();
                }
            }
            if (!listTProveedores.Count.Equals(0))
            {
                foreach (var item in listTProveedores)
                {
                    if (item.Enabled)
                    {
                        var correoProveedor = new TCorreosProveedor();
                        var telefonoProveedor = new TTelefonosProveedor();
                        var direccionProveedor = new TDireccionesProveedor();
                        var provincia = new TProvincia();
                        var canton = new TCanton();
                        var distrito = new TDistrito();
                        correoProveedor = _context.TCorreosProveedor.Where(c => c.proveedor.Ced_Jur.Equals(item.Ced_Jur)).ToList().Last();
                        telefonoProveedor = _context.TTelefonosProveedor.Where(t => t.proveedor.Ced_Jur.Equals(item.Ced_Jur)).ToList().Last();
                        direccionProveedor = _context.TDireccionesProveedor.Where(d => d.proveedor.Ced_Jur.Equals(item.Ced_Jur)).ToList().Last();
                        if (direccionProveedor != null)
                        {
                            provincia = provincias.Where(p => p.idProvincia.Equals(direccionProveedor.TProvincia.idProvincia)).ToList().Last();
                            canton = cantones.Where(c => c.idCanton.Equals(direccionProveedor.TCanton.idCanton)).ToList().Last();
                            distrito = distritos.Where(d => d.idDistrito.Equals(direccionProveedor.idDistrito)).ToList().Last();
                        }

                        listaProveedor.Add(new InputModelProveedor
                        {
                            idProveedor = item.IdProveedor,
                            CedulaJuridica = item.Ced_Jur,
                            NombreProveedor = item.Nombre_Proveedor,
                            Image = item.Imagen,
                            Email = correoProveedor.correo,
                            Telefono = telefonoProveedor.telefono,
                            Direccion = direccionProveedor.Direccion,
                            nombreCanton = canton.nombreCanton,
                            nombreProvincia = provincia.nombreProvincia,
                            nombreDistrito = distrito.nombreDistrito
                        });
                    }
                }
            }
            return listaProveedor;
        }

        public bool NoExisteCedJur(long Cedula)
        {
            bool result = false;
            var proveedor = _context.TProveedor.Where(p => p.Ced_Jur.Equals(Cedula)).ToList();
            if (proveedor.Count.Equals(0))
            {
                return result = true;
            }
            else
            {
                return result = false;
            }
        }

        public string getCorreoProveedor(long Cedula)
        {
            TCorreosProveedor listCorreoProveedor = _context.TCorreosProveedor.Where(c => c.proveedor.Ced_Jur.Equals(Cedula)).ToList().Last();
            return listCorreoProveedor.correo;
        }

        public string getTelefonoProveedor(long Cedula)
        {
            TTelefonosProveedor listTelefonoProveedor = _context.TTelefonosProveedor.Where(c => c.proveedor.Ced_Jur.Equals(Cedula)).ToList().Last();
            return listTelefonoProveedor.telefono;
        }

        public List<TProveedor> getProveedor(long Cedula)
        {
            var listTProveedores = new List<TProveedor>();
            using (var dbContext = new ApplicationDbContext())
            {
                listTProveedores = dbContext.TProveedor.Where(u => u.Ced_Jur.Equals(Cedula)).ToList();
            }
            return listTProveedores;
        }

        public InputModelProveedor getDeudaProveedor (long id)
        {
            var datosProveedor = new InputModelProveedor();

            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.TProveedor.Join(dbContext.TCreditoProveedor, p => p.IdProveedor, c => c.TProveedor.IdProveedor, (p, c) => new
                {
                    p.IdProveedor,
                    p.Ced_Jur,
                    p.Nombre_Proveedor,
                    c.idCredito,
                    c.Deuda,
                    c.Mensual,
                    c.Cambio,
                    c.DeudaActual,
                    c.FechaPago,
                    c.UltimoPago,
                    c.Ticket,
                    c.FechaLimite,
                    c.FechaDeuda
                }).ToList();
                if (!id.Equals(0))
                {
                    query = query.Where(c => c.Ced_Jur.Equals(id)).ToList();
                    if (!query.Count.Equals(0))
                    {
                        var data = query.ToList().Last();
                        datosProveedor = new InputModelProveedor
                        {
                            idProveedor = data.IdProveedor,
                            NombreProveedor = data.Nombre_Proveedor,
                            CedulaJuridica = data.Ced_Jur,
                            Telefono = getCorreoProveedor(data.Ced_Jur),
                            Email = getCorreoProveedor(data.Ced_Jur),
                            idCredito = data.idCredito,
                            Deuda = data.Deuda,
                            Mensual = data.Mensual,
                            Cambio = data.Cambio,
                            DeudaActual = data.DeudaActual,
                            FechaPago = data.FechaPago,
                            FechaDeuda = data.FechaDeuda,
                            UltimoPago = data.UltimoPago,
                            Ticket = data.Ticket,
                            FechaLimite = data.FechaLimite
                        };
                    }
                }
            }
            return datosProveedor;
        }

        public DataPaginador<THistoricoPagosProveedor> GetPagosListaProveedor(long id, int page, int num, InputModelProveedor input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetPagosProveedor(input, id);
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<THistoricoPagosProveedor>().paginador(data, page, 10, "Proveedores", "Proveedores", "Proveedores/Credito", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<THistoricoPagosProveedor>();
            }
            var models = new DataPaginador<THistoricoPagosProveedor>
            {
                List = (List<THistoricoPagosProveedor>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new THistoricoPagosProveedor(),
            };

            return models;
        }

        public List<THistoricoPagosProveedor> GetPagosProveedor(InputModelProveedor input, long id)
        {
            var listPagos = new List<THistoricoPagosProveedor>();
            var listPagos2 = new List<THistoricoPagosProveedor>();
            /*
             Menor a cero: si t1 es anterior a t2
             Cero: si t1 es lo mismo que t2
             Mayor a cero: si t1 es posterior a t2
             */
            var t1 = input.horaInicio.ToString("dd/MMM/yyy");
            var t2 = input.horaFinal.ToString("dd/MMM/yyy");

            int fechaCompare = DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2));

            if (fechaCompare > 0)
            {
                listPagos2 = _context.THistoricoPagosProveedor.Where(h => h.proveedor.Ced_Jur.Equals(id)).ToList();
            }
            else
            {
                if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
                {
                    listPagos2 = _context.THistoricoPagosProveedor.Where(h => h.proveedor.Ced_Jur.Equals(id)).ToList();
                }
                else
                {
                    foreach (var item in _context.THistoricoPagosProveedor.Where(h => h.proveedor.Ced_Jur.Equals(id)).ToList())
                    {
                        int fecha1 = DateTime.Compare(DateTime.Parse(item.Fecha.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                        if (fecha1.Equals(0) || fecha1 > 0)
                        {
                            listPagos.Add(item);
                        }
                    }
                    foreach (var item in listPagos)
                    {
                        int fecha2 = DateTime.Compare(DateTime.Parse(item.Fecha.ToString("dd/MMM/yyy")), DateTime.Parse(t2));
                        if (fecha2.Equals(0) || fecha2 < 0)
                        {
                            listPagos2.Add(item);
                        }
                    }
                }
            }
            return listPagos2;
        }

        public InputModelProveedor getPagoProveedor (int idPago)
        {
            var dataProveedores = new InputModelProveedor();
            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.THistoricoPagosProveedor.Join(dbContext.TProveedor, h => h.proveedor.Ced_Jur, p => p.Ced_Jur, (h, p) => new
                {
                    p.IdProveedor,
                    p.Ced_Jur,
                    p.Nombre_Proveedor,
                    h.IdPago,
                    h.Deuda,
                    h.Pago,
                    h.Cambio,
                    h.DeudaActual,
                    h.Fecha,
                    h.FechaLimite,
                    h.FechaDeuda,
                    h.Mensual,
                    h.DeudaAnterior,
                    h.Ticket,
                    h.IdUser,
                    h.User
                }).Where(h => h.IdPago.Equals(idPago)).ToList();

                if (!query.Count().Equals(0))
                {
                    var data = query.ToList().Last();
                    dataProveedores = new InputModelProveedor
                    {
                        idProveedor = data.IdProveedor,
                        CedulaJuridica = data.Ced_Jur,
                        NombreProveedor = data.Nombre_Proveedor,
                        Telefono = getTelefonoProveedor(data.Ced_Jur),
                        Email = getCorreoProveedor(data.Ced_Jur),
                        IdPago = data.IdPago,
                        Deuda = data.Deuda,
                        Pago = data.Pago,
                        Cambio = data.Cambio,
                        DeudaActual = data.DeudaActual,
                        FechaPago = data.Fecha,
                        Ticket = data.Ticket,
                        FechaLimite = data.FechaLimite,
                        FechaDeuda = data.FechaDeuda,
                        Mensual = data.Mensual,
                        DeudaAnterior = data.DeudaAnterior,
                        IdUser = data.IdUser,
                        User = data.User,
                    };

                }
            }
            return dataProveedores;
        }

    }
}
