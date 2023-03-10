using Azure.Core;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FactuLib.Library
{
    public class LCliente : ListObject
    {
        public LCliente(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<InputModelRegistrar> getTClienteAsync(String valor, long id)
        {
            List<TClientes> listTClientes;
            TDireccionCliente _listTDireccionCliente;            
            var clientesList = new List<InputModelRegistrar>();
            string nombreDistrito = "";
            string nombreCanton = "";
            string nombreProvincia = "";
            string direccionExacta = "";
            string correoCliente = "";
            if (valor == null && id.Equals(0))
            {
                listTClientes = _context.TClientes.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    //listTClientes = _context.TClientes.Where(u => u.Nombre.StartsWith(valor) || u.Apellido.StartsWith(valor) ||
                    //u.Correo.StartsWith(valor)).ToList();

                   var query = _context.TClientes.Join(_context.TCorreosClientes, c=>c.Cedula, e=>e.cliente.Cedula, (c,e) => new 
                    { 
                        c.Cedula,
                        c.Nombre,
                        c.Apellido1,
                        c.Apellido2,
                        c.Fecha,
                        c.Credito,
                        c.Imagen,
                        c.Enabled,
                        e.correo
                    }).Where(c => c.Nombre.StartsWith(valor) || c.Apellido1.StartsWith(valor) || c.Apellido2.StartsWith(valor) || c.correo.StartsWith(valor)).ToList();

                    listTClientes = new List<TClientes>();
                    foreach (var item in query)
                    {
                        listTClientes.Add(new TClientes
                        {
                            Cedula = item.Cedula,
                            Nombre = item.Nombre,
                            Apellido1 = item.Apellido1,
                            Apellido2 = item.Apellido2,
                            Fecha = item.Fecha,
                            Credito = item.Credito,
                            Imagen = item.Imagen,
                            Enabled = item.Enabled
                        });
                    }
                }
                else
                {
                    listTClientes = _context.TClientes.Where(u => u.Cedula.Equals(id)).ToList();

                    _listTDireccionCliente = _context.TDireccionCliente.Where(d => d.clientes.Cedula.Equals(id)).ToList().Last();
                    var _queryDireccion = _context.TDistrito.Join(_context.TCanton, d => d.canton.idCanton, c => c.idCanton, (d, c) => new
                    {
                        d.idDistrito,
                        d.nombreDistrito,
                        c.idCanton,
                        c.nombreCanton,
                        c.provincia.idProvincia
                    }).Where(d => d.idDistrito.Equals(_listTDireccionCliente.idDistrito)).ToList().Last();
                    var _provincia = _context.TProvincia.Where(p => p.idProvincia.Equals(_queryDireccion.idProvincia)).ToList().Last();
                    nombreDistrito = _queryDireccion.nombreDistrito;
                    nombreCanton = _queryDireccion.nombreCanton;
                    nombreProvincia = _provincia.nombreProvincia;
                    direccionExacta = _listTDireccionCliente.Direccion;
                    correoCliente = getCorreoCliente(id);
                }
            }
            if (!listTClientes.Count.Equals(0))
            {
                foreach (var item in listTClientes)
                {
                    if (item.Enabled)
                    {
                        clientesList.Add(new InputModelRegistrar
                        {
                            Cedula = item.Cedula,
                            Name = item.Nombre,
                            Apellido1 = item.Apellido1,
                            Apellido2 = item.Apellido2,
                            Email = getCorreoCliente(item.Cedula),
                            Phone = getTelefonoCliente(item.Cedula),
                            Credit = item.Credito,
                            Direction = direccionExacta,
                            Image = item.Imagen,
                            nombreDistrito = nombreDistrito,
                            nombreCanton = nombreCanton,
                            nombreProvincia = nombreProvincia,
                            direccionExacta = direccionExacta
                        });
                    }    
                }
            }
            return clientesList;
        }

        public List<TClientes> getCliente(long Cedula)
        {
            var listTClientes = new List<TClientes>();
            using (var dbContext = new ApplicationDbContext())
            {
                listTClientes = dbContext.TClientes.Where(u => u.Cedula.Equals(Cedula)).ToList();
            }
            return listTClientes;
        }

        public String getCorreoCliente (long Cedula)
        {
           TCorreosClientes listCorreosClientes = _context.TCorreosClientes.Where(c => c.cliente.Cedula.Equals(Cedula)).ToList().Last();
            return listCorreosClientes.correo;
        }

        public String getTelefonoCliente(long Cedula)
        {
            TTelefonoCliente listtelefonoCliente = _context.TTelefonoCliente.Where(c => c.clientes.Cedula.Equals(Cedula)).ToList().Last();
            return listtelefonoCliente.telefono;
        }

        public InputModelRegistrar getClienteConCredito(long Cedula)
        {
            var dataClients = new InputModelRegistrar();
            using (var dbContext = new ApplicationDbContext())
            {
                TCorreosClientes listCorreosClientes = _context.TCorreosClientes.Where(c => c.cliente.Cedula.Equals(Cedula)).ToList().Last();
                string correoCliente = listCorreosClientes.correo;
                var query = dbContext.TClientes.Join(dbContext.TCreditoClientes, c => c.Cedula, r => r.TClients.Cedula, (c, r) => new
                {
                    c.Cedula,
                    c.Nombre,
                    c.Apellido1,
                    c.Apellido2,
                    c.Credito,
                    r.idDeuda,
                    r.Deuda,
                    r.Mensual,
                    r.Cambio,
                    r.DeudaActual,
                    r.FechaPago,
                    r.UltimoPago,
                    r.Ticket,
                    r.FechaLimite
                }).Where(c => c.Cedula.Equals(Cedula)).ToList();

                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new InputModelRegistrar
                    {
                        Cedula = data.Cedula,
                        Name = data.Nombre,
                        Apellido1 = data.Apellido1,
                        Apellido2 = data.Apellido2,
                        Phone = getTelefonoCliente(data.Cedula),
                        Email = getCorreoCliente(data.Cedula),
                        Credit = data.Credito,
                        idDeuda = data.idDeuda,
                        Deuda = data.Deuda,
                        Mensual = data.Mensual,
                        Cambio = data.Cambio,
                        DeudaActual = data.DeudaActual,
                        FechaPago = data.FechaPago,
                        Ticket = data.Ticket,
                        FechaLimite = data.FechaLimite
                    };
                }
            }
            return dataClients;
        }


        public InputModelRegistrar getClienteCredito(long id)
        {
            var dataClients = new InputModelRegistrar();
            using (var dbContext = new ApplicationDbContext())
            {
                
                TCorreosClientes listCorreosClientes = _context.TCorreosClientes.Where(c => c.cliente.Cedula.Equals(id)).ToList().Last();
                string correoCliente = listCorreosClientes.correo;
                var query = dbContext.TClientes.Join(dbContext.TCreditoClientes, c => c.Cedula, r => r.TClients.Cedula, (c, r) => new
                {
                    c.Cedula,
                    c.Nombre,
                    c.Apellido1,
                    c.Apellido2,    
                    c.Credito,
                    r.idDeuda,
                    r.Deuda,
                    r.Mensual,
                    r.Cambio,
                    r.DeudaActual,
                    r.FechaPago,
                    r.UltimoPago,
                    r.Ticket,
                    r.FechaLimite
                }).Where(c => c.Cedula.Equals(id)).ToList();
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    dataClients = new InputModelRegistrar
                    {
                        Cedula = data.Cedula,
                        Name = data.Nombre,
                        Apellido1 = data.Apellido1,
                        Apellido2 = data.Apellido2,
                        Phone = getTelefonoCliente(data.Cedula),
                        Email = getCorreoCliente(data.Cedula),
                        Credit = data.Credito,
                        idDeuda = data.idDeuda,
                        Deuda = data.Deuda,
                        Mensual = data.Mensual,
                        Cambio = data.Cambio,
                        DeudaActual = data.DeudaActual,
                        FechaPago = data.FechaPago,
                        Ticket = data.Ticket,
                        FechaLimite = data.FechaLimite
                    };
                }
            }
            return dataClients;
        }

        public String validacionFecha(InputModelRegistrar input)
        {
            var t1 = input.horaInicio.ToString("dd/MMM/yyy");
            var t2 = input.horaFinal.ToString("dd/MMM/yyy");
            int fechaCompare = DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2));
            string error;

            if (fechaCompare > 0)
            {
                error= "La fecha de Inicio debe ser menor que la fecha final";
            }
            else
            {
                error = "";
            }
            return error;
        }

        public DataPaginador<THistoricoPagosClientes> GetPagos(long id, int page,int num, InputModelRegistrar input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetPagosClientes(input, id);
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<THistoricoPagosClientes>().paginador(data, page, 10, "Clientes", "Clientes", "Clientes/CreditoCliente",url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<THistoricoPagosClientes>();
            }
            var models = new DataPaginador<THistoricoPagosClientes>
            {
                List = (List<THistoricoPagosClientes>)objects[2],
                Pagi_info = (String) objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new THistoricoPagosClientes(),
            };

            return models;
        }

        public DataPaginador<THistoricoPagosClientes> GetPagosReporteCliente(long id, int page, int num, InputModelRegistrar input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            _context.THistoricoPagosClientes.Where(h => h.CedulaCliente.Equals(id)).ToList();
            var data = _context.THistoricoPagosClientes.Where(h => h.CedulaCliente.Equals(id)).ToList();
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<THistoricoPagosClientes>().paginador(data, page, 10, "Clientes", "Clientes", "Clientes/CreditoCliente", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<THistoricoPagosClientes>();
            }
            var models = new DataPaginador<THistoricoPagosClientes>
            {
                List = (List<THistoricoPagosClientes>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new THistoricoPagosClientes(),
            };

            return models;
        }

        public List<THistoricoPagosClientes> GetPagosClientes ( InputModelRegistrar input, long id)
        {
            var listPagos = new List<THistoricoPagosClientes>();
            var listPagos2 = new List<THistoricoPagosClientes>();
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
                listPagos2 = _context.THistoricoPagosClientes.Where(h => h.CedulaCliente.Equals(id)).ToList();
            }
            else
            {
                if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
                {
                    listPagos2 = _context.THistoricoPagosClientes.Where(h => h.CedulaCliente.Equals(id)).ToList();
                }
                else
                {
                    foreach (var item in _context.THistoricoPagosClientes.Where(h => h.CedulaCliente.Equals(id)).ToList())
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

        public InputModelRegistrar getTClientesPagos(int idPago)
        {
            var dataClients = new InputModelRegistrar();
            using (var dbContext = new ApplicationDbContext())
            {
                
                var query = dbContext.THistoricoPagosClientes.Join(dbContext.TClientes, h => h.CedulaCliente, c => c.Cedula, (h, c) => new
                {
                    c.Cedula,
                    c.Nombre,
                    c.Apellido1,
                    c.Apellido2,
                    c.Credito,
                    h.IdPago,
                    h.Deuda,
                    h.Pago,
                    h.Cambio,    
                    h.DeudaActual,
                    h.Fecha,
                    h.FechaLimite,
                    h.Ticket,
                    h.IdUser,
                    h.User
                }).Where(h => h.IdPago.Equals(idPago)).ToList();
                
                if (!query.Count.Equals(0))
                {
                    var data = query.ToList().Last();
                    var queryDeuda = dbContext.TCreditoClientes.Where(c => c.TClients.Cedula.Equals(data.Cedula)).ToList().Last();
                    dataClients = new InputModelRegistrar 
                    { 
                        Cedula = data.Cedula,
                        Name = data.Nombre,
                        Apellido1 = data.Apellido1,
                        Apellido2 = data.Apellido2,
                        Phone = getTelefonoCliente(data.Cedula),
                        Email = getCorreoCliente(data.Cedula),
                        Credit = data.Credito,
                        IdPago = data.IdPago,
                        Deuda = data.Deuda,
                        Mensual = queryDeuda.Mensual,
                        FechaDeuda = queryDeuda.FechaDeuda,
                        Pago = data.Pago,
                        Cambio = data.Cambio,
                        DeudaActual = data.DeudaActual,
                        FechaPago = data.Fecha,
                        Ticket = data.Ticket,
                        FechaLimite = data.FechaLimite,
                        IdUser = data.IdUser,
                        User = data.User
                    };
                }
            }
            return dataClients;
        }

        public bool ExisteCliente(long Cedula)
        {
            bool resultado = false;
            var cliente = _context.TClientes.Where(c => c.Cedula.Equals(Cedula)).ToList();

            if (cliente.Count > 0)
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }

        public string getNombreCliente(int id)
        {
            string nombreCliente = "";
            if (id > 0)
            {
                var cliente = _context.TClientes.Where(p => p.IdCliente.Equals(id)).ToList().Last();
                if (cliente.IdCliente > 0)
                {
                    nombreCliente = cliente.Nombre + " " + cliente.Apellido1;
                }
            }
            return nombreCliente;
        }
    }
}
