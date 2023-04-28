using FactuLib.Areas.Cajas.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactuLib.Library
{
    public class LCierre : ListObject
    {
        public LCierre(ApplicationDbContext context)
        {
            _context = context;
        }

        public float calculaMontoFaltanteCajas(float montoCajasInicial, float montoCajasFinal, float montoDineroRecibidoVentas, float montoDineroRecibidoCompras, float cambioVentas, float cambioCompras)
        {
            float resultado = 0;
            float dineroSaliente = montoDineroRecibidoCompras + cambioVentas;
            float dineroEntrante = montoCajasInicial + montoDineroRecibidoVentas + cambioCompras;
            float arqueo = dineroEntrante - dineroSaliente;
            resultado = montoCajasFinal - arqueo;

            if (resultado < 0)
            {

                return resultado;
            }
            else
            {
                return 0;
            }

        }

        public float calculaMontoSobranteCajas(float montoCajasInicial, float montoCajasFinal, float montoDineroRecibidoVentas, float montoDineroRecibidoCompras, float cambioVentas, float cambioCompras)
        {
            float resultado = 0;
            float dineroSaliente = montoDineroRecibidoCompras + cambioVentas;
            float dineroEntrante = montoCajasInicial + montoDineroRecibidoVentas + cambioCompras;
            float arqueo = dineroEntrante - dineroSaliente;
            resultado = montoCajasFinal - arqueo;

            if (resultado > 0)
            {

                return resultado;
            }
            else
            {
                return 0;
            }
        }

        public float calculaMontoFaltanteCuentas(float montoCuentasInicial, float montoCuentasFinal, float montoVentasCuentas, float montoComprasCuentas)
        {
            float resultado = 0;
            float dineroSaliente = montoComprasCuentas;
            float dineroEntrante = montoCuentasInicial + montoVentasCuentas;
            float arqueo = dineroEntrante - dineroSaliente;
            resultado = montoCuentasFinal - arqueo;

            if (resultado < 0)
            {

                return resultado;
            }
            else
            {
                return 0;
            }
        }

        public float calculaMontoSobranteCuentas(float montoCuentasInicial, float montoCuentasFinal, float montoVentasCuentas, float montoComprasCuentas)
        {
            float resultado = 0;
            float dineroSaliente = montoComprasCuentas;
            float dineroEntrante = montoCuentasInicial + montoVentasCuentas;
            float arqueo = dineroEntrante - dineroSaliente;
            resultado = montoCuentasFinal - arqueo;

            if (resultado > 0)
            {

                return resultado;
            }
            else
            {
                return 0;
            }
        }


        public DataPaginador<TRegistroCierre> GetRegistroCierres(int page, int num, InputModelCierre input, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = GetRegistroCierre(input);
            if (data.Count > 0)
            {
                data.Reverse();
                objects = new LPaginador<TRegistroCierre>().paginador(data, page, 5, "Cajas", "Cajas", "Lista_Cierres", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TRegistroCierre>();
            }
            var models = new DataPaginador<TRegistroCierre>
            {
                List = (List<TRegistroCierre>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TRegistroCierre(),
            };
            return models;
        }


        public List<TRegistroCierre> GetRegistroCierre(InputModelCierre input)
        {
            var listCierres = new List<TRegistroCierre>();
            var listCierres2 = new List<TRegistroCierre>();
            /*
             Menor a cero: si t1 es anterior a t2
             Cero: si t1 es lo mismo que t2
             Mayor a cero: si t1 es posterior a t2
             */
            var t1 = input.horaInicio.ToString("dd/MMM/yyy");
            var t2 = input.horaFinal.ToString("dd/MMM/yyy");
            var aperturas = _context.TRegisroApertura.ToList();
            var usuarios = _context.TUsers.ToList();

            int fechaCompare = DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2));

            if (fechaCompare > 0)
            {
                listCierres2 = _context.TRegisroCierre.ToList();
            }
            else
            {
                if (t1.Equals(t2) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t1) && DateTime.Now.ToString("dd/MMM/yyy").Equals(t2))
                {
                    listCierres2 = _context.TRegisroCierre.ToList();
                }
                else
                {
                    foreach (var item in _context.TRegisroCierre.ToList())
                    {
                        int fecha1 = DateTime.Compare(DateTime.Parse(item.Fecha_Cierre.ToString("dd/MMM/yyy")), DateTime.Parse(t1));
                        if (fecha1.Equals(0) || fecha1 > 0)
                        {
                            listCierres.Add(item);
                        }
                    }
                    foreach (var item in listCierres)
                    {
                        int fecha2 = DateTime.Compare(DateTime.Parse(item.Fecha_Cierre.ToString("dd/MMM/yyy")), DateTime.Parse(t2));
                        if (fecha2.Equals(0) || fecha2 < 0)
                        {
                            listCierres2.Add(item);
                        }
                    }
                }
            }
            return listCierres2;
        }

        public string estadoCajaActual()
        {
            string estadoCaja;
            var listaApertura = _context.TRegisroApertura.Where(r => r.Estado.Equals(true)).ToList();
            if (listaApertura.Count > 0)
            {
                estadoCaja = "Abierta";
            }
            else
            {
                estadoCaja = "Cerrada";
            }
            return estadoCaja;
        }

    }
}
