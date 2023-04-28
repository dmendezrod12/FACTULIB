using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Library
{
    public class LPaginador <T>
    {
        //Cantidad de resultados por pagina
        private int pagi_cuantos = 10;
        //Cantidad de enlaces que se mostraran como maximo en la barra de navegación
        private int pagi_nav_num_enlaces = 3;
        private int pagi_actual;
        //Se define que ira en el enlace de la página anterior
        private String pagi_nav_anterior = "&laquo; Anterior ";
        //Se define que ira en el enlace de la página siguiente
        private String pagi_nav_siguiente = "Siguiente &raquo ";
        //Se define que ira en el enlace a la pagina siguiente
        private String pagi_nav_primera = "&laquo; Primero ";
        private String pagi_nav_ultima = " Ultimo &raquo; ";
        private String pagi_navegacion = null;

        public object[] paginador(List<T> table, int pagina, int registros,  String area, String controller, String action, String host)
        {
            pagi_actual = pagina == 0 ? 1 : pagina;
            if (registros>0)
            {
                pagi_cuantos = registros;
            }

            int pagi_totalReg = table.Count;
            double valor1 = Math.Ceiling((double) pagi_totalReg / (double) pagi_cuantos);
            int pagi_totalPags = Convert.ToInt16(Math.Ceiling(valor1));
            if (pagi_actual != 1)
            {
                //Si no se encuentra en la pagina 1. Se coloca el enlace "Primera"
                int pagi_url = 1; //sera el numero de página enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" + 
                    pagi_url + "&registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_primera + "</a>";

                //Si no se encuentra en la pagina 1. Se coloca el enlace "Anterior"
                pagi_url = pagi_actual -1; //sera el numero de página enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" +
                    pagi_url + "&registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_anterior + "</a>";
            }

            //Si se definio la variable pagi_nav_num_enlaces
            // Se calcula el intervalo para restar y sumar a partir de la pagina actual

            double valor2 = (pagi_nav_num_enlaces / 2);
            int pagi_nav_intervalo = Convert.ToInt16(Math.Round(valor2));

            //Calcula desde que numero de pagina se mostrara

            int pagi_nav_desde = pagi_actual - pagi_nav_intervalo;

            //Calcula hasta que numero de pagina se mostrara

            int pagi_nav_hasta = pagi_actual + pagi_nav_intervalo;

            //Validacion en caso de que pagi_nav_desde sea numero negativo

            if (pagi_nav_desde < -1)
            {
                //Se le suma la cantidad sobrante al final para mantener
                //el número de enlaces que se quiere mostrar

                pagi_nav_hasta -= (pagi_nav_desde - 1);
                                                  
                //Se establece pagi_nav_desde como 1

                pagi_nav_desde = 1;
            }

            // Validacion en caso de que pagi_nav_hasta es un numero mayor que el total de paginas

            if (pagi_nav_hasta > pagi_totalPags)
            {
                //Se resta la cantidad excedida al comienzo para mantener
                //el número de enlaces que se quiere mostrar

                pagi_nav_desde -= (pagi_nav_hasta - pagi_totalPags);
                // Se establece pagi_nav_hasta como el total de pagians

                pagi_nav_hasta = pagi_totalPags;

                // Se realiza un ajuste para verificar que al cambiar pag_nav_desde
                // no haya quedado con un valor no valido

                if (pagi_nav_desde < 1)
                {
                    pagi_nav_desde = 1; 
                }
            }

            for (int pagi_i = pagi_nav_desde; pagi_i <=  pagi_nav_hasta; pagi_i++)
            {
                //Desde pagina 1 hasta ultima pagina (pagi_totalPags)

                if (pagi_i == pagi_actual)
                {
                    //Si el numero de pagina es la actual (pagi_actual). Se escribe el numero, pero sin enlace
                    pagi_navegacion += "<span class='btn btn-default' disabled='disabled'>" + pagi_i + "</span>";
                }
                else
                {
                    //Si es cualquier otro. Se escribe el enlace a dicho numero de pagina
                    pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action +
                        "?id=" + pagi_i + "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_i + " </a>";
                }
            }

            if (pagi_actual < pagi_totalPags)
            {
                // Si no se encuentra en la ultima pagina. Se coloca el enlace "Siguiente"
                int pagi_url = pagi_actual + 1; //Sera el numero de pagina enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url + "&Registros=" + pagi_cuantos
                    + "&area=" + area + "'>" + pagi_nav_siguiente + "</a>";

                //Si no se encuentra en la ultima pagina. Se coloca el enlace ultima
                pagi_url = pagi_totalPags; // sera el numero de página al que se enlaza
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url + "&Registros=" + pagi_cuantos
                    + "&area=" + area + "'>" + pagi_nav_ultima + "</a>";

            }

            /// Obtención de registros que se mostraran en la página actual
            /// Se calcula desde que registro se mostrara en la pagina
            /// Recordar que el conteo comienza desde 0

            int pagi_inicial = (pagi_actual - 1) * pagi_cuantos;

            // Consulta SQL. Devuelve cantidad registros empezando desde pagi_inicial

            var query = table.Skip(pagi_inicial).Take(pagi_cuantos).ToList();

            //Numero del primer registro de la pagina actual

            int pagi_desde = pagi_inicial + 1;

            //Numero del ultimo registro de la pagina actual
            int pagi_hasta = pagi_inicial + pagi_cuantos;

            if (pagi_hasta > pagi_totalReg)
            {
                //Si se encuentra en la ultima pagina el ultimo registro de la pagina actual sera igual al numero de registros
                pagi_hasta = pagi_totalReg;

            }

            

            /*Genera info de registros mostrados*/
            String pagi_info = "del <b>" + pagi_actual + "</b> al <b>" + pagi_totalPags + "</b> de <b>" + pagi_totalReg + "</b> <b>/" + pagi_cuantos + " </b>";
            object[] data = { pagi_info, pagi_navegacion, query };
            return data;
        }

        public object[] paginadorDetalle(int idDetalle, List<T> table, int pagina, int registros, String area, String controller, String action, String host)
        {
            pagi_actual = pagina == 0 ? 1 : pagina;
            if (registros > 0)
            {
                pagi_cuantos = registros;
            }

            int pagi_totalReg = table.Count;
            double valor1 = Math.Ceiling((double)pagi_totalReg / (double)pagi_cuantos);
            int pagi_totalPags = Convert.ToInt16(Math.Ceiling(valor1));
            if (pagi_actual != 1)
            {
                //Si no se encuentra en la pagina 1. Se coloca el enlace "Primera"
                int pagi_url = 1; //sera el numero de página enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" +
                    pagi_url + "&idCompra=" + idDetalle +"&registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_primera + "</a>";

                //Si no se encuentra en la pagina 1. Se coloca el enlace "Anterior"
                pagi_url = pagi_actual - 1; //sera el numero de página enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" +
                    pagi_url + "&idCompra=" + idDetalle + "&registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_nav_anterior + "</a>";
            }

            //Si se definio la variable pagi_nav_num_enlaces
            // Se calcula el intervalo para restar y sumar a partir de la pagina actual

            double valor2 = (pagi_nav_num_enlaces / 2);
            int pagi_nav_intervalo = Convert.ToInt16(Math.Round(valor2));

            //Calcula desde que numero de pagina se mostrara

            int pagi_nav_desde = pagi_actual - pagi_nav_intervalo;

            //Calcula hasta que numero de pagina se mostrara

            int pagi_nav_hasta = pagi_actual + pagi_nav_intervalo;

            //Validacion en caso de que pagi_nav_desde sea numero negativo

            if (pagi_nav_desde < -1)
            {
                //Se le suma la cantidad sobrante al final para mantener
                //el número de enlaces que se quiere mostrar

                pagi_nav_hasta -= (pagi_nav_desde - 1);

                //Se establece pagi_nav_desde como 1

                pagi_nav_desde = 1;
            }

            // Validacion en caso de que pagi_nav_hasta es un numero mayor que el total de paginas

            if (pagi_nav_hasta > pagi_totalPags)
            {
                //Se resta la cantidad excedida al comienzo para mantener
                //el número de enlaces que se quiere mostrar

                pagi_nav_desde -= (pagi_nav_hasta - pagi_totalPags);
                // Se establece pagi_nav_hasta como el total de pagians

                pagi_nav_hasta = pagi_totalPags;

                // Se realiza un ajuste para verificar que al cambiar pag_nav_desde
                // no haya quedado con un valor no valido

                if (pagi_nav_desde < 1)
                {
                    pagi_nav_desde = 1;
                }
            }

            for (int pagi_i = pagi_nav_desde; pagi_i <= pagi_nav_hasta; pagi_i++)
            {
                //Desde pagina 1 hasta ultima pagina (pagi_totalPags)

                if (pagi_i == pagi_actual)
                {
                    //Si el numero de pagina es la actual (pagi_actual). Se escribe el numero, pero sin enlace
                    pagi_navegacion += "<span class='btn btn-default' disabled='disabled'>" + pagi_i + "</span>";
                }
                else
                {
                    //Si es cualquier otro. Se escribe el enlace a dicho numero de pagina
                    pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action +
                        "?id=" + pagi_i + "&idCompra=" + idDetalle + "&Registros=" + pagi_cuantos + "&area=" + area + "'>" + pagi_i + " </a>";
                }
            }

            if (pagi_actual < pagi_totalPags)
            {
                // Si no se encuentra en la ultima pagina. Se coloca el enlace "Siguiente"
                int pagi_url = pagi_actual + 1; //Sera el numero de pagina enlazado
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url + "&idCompra=" + idDetalle + "&Registros=" + pagi_cuantos
                    + "&area=" + area + "'>" + pagi_nav_siguiente + "</a>";

                //Si no se encuentra en la ultima pagina. Se coloca el enlace ultima
                pagi_url = pagi_totalPags; // sera el numero de página al que se enlaza
                pagi_navegacion += "<a class='btn btn-default' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url + "&idCompra=" + idDetalle + "&Registros=" + pagi_cuantos
                    + "&area=" + area + "'>" + pagi_nav_ultima + "</a>";

            }

            /// Obtención de registros que se mostraran en la página actual
            /// Se calcula desde que registro se mostrara en la pagina
            /// Recordar que el conteo comienza desde 0

            int pagi_inicial = (pagi_actual - 1) * pagi_cuantos;

            // Consulta SQL. Devuelve cantidad registros empezando desde pagi_inicial

            var query = table.Skip(pagi_inicial).Take(pagi_cuantos).ToList();

            //Numero del primer registro de la pagina actual

            int pagi_desde = pagi_inicial + 1;

            //Numero del ultimo registro de la pagina actual
            int pagi_hasta = pagi_inicial + pagi_cuantos;

            if (pagi_hasta > pagi_totalReg)
            {
                //Si se encuentra en la ultima pagina el ultimo registro de la pagina actual sera igual al numero de registros
                pagi_hasta = pagi_totalReg;

            }



            /*Genera info de registros mostrados*/
            String pagi_info = "del <b>" + pagi_actual + "</b> al <b>" + pagi_totalPags + "</b> de <b>" + pagi_totalReg + "</b> <b>/" + pagi_cuantos + " </b>";
            object[] data = { pagi_info, pagi_navegacion, query };
            return data;
        }


    }
}
