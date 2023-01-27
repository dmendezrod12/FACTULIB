using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FactuLib.Library
{
    public class LTicket
    {
        private StringBuilder lineas = new StringBuilder();
        private int maxCaracter = 40, stop;

        public String LineasGuion()
        {
            string linea = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                linea += "-";
            }
            return lineas.AppendLine(linea).ToString();
        }

        public String LineaAsteriscos()
        {
            string asterisco = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                asterisco += "*";
            }
            return lineas.AppendLine(asterisco).ToString();
        }

        public String LineaIgual()
        {
            string igual = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                igual += "=";
            }
            return lineas.AppendLine(igual).ToString();
        }

        public void EncabezadoVenta(String columnas)
        {
            lineas.AppendLine(columnas);
        }

        public void TextoIzquierda(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                lineas.AppendLine(texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                lineas.AppendLine(texto);
            }
        }

        public void TextoDerecha(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                String espacios = "";
                for (int i = 0; i < (maxCaracter - texto.Substring(caracterActual, texto.Length - caracterActual).Length); i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));

            }
            else
            {
                String espacios = "";
                for (int i = 0; i < (maxCaracter - texto.Length); i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto);
            }
        }

        public void TextoCentro(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                String espacios = "";
                int centrar = (maxCaracter - texto.Substring(caracterActual, texto.Length - caracterActual).Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                String espacios = "";
                int centrar = (maxCaracter - texto.Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto);
            }
        }

        public void TextoExtremo(String izquierdo, String derecho)
        {
            String der, izq, completo = "", espacio = "";
            if (izquierdo.Length > 18)
            {
                stop = izquierdo.Length - 18;
                izq = izquierdo.Remove(18, stop);
            }
            else
            {
                izq = izquierdo;
            }
            completo = izq;
            if (derecho.Length > 20)
            {
                stop = derecho.Length - 20;
                der = derecho.Remove(20, stop);
            }
            else
            {
                der = derecho;
            }
            int numEspacios = maxCaracter - (izq.Length + der.Length);
            for (int i = 0; i < numEspacios; i++)
            {
                espacio += " ";
            }
            completo += espacio + derecho;
            lineas.AppendLine(completo);
        }

        public void AgregarTotales(String texto, String total)
        {
            String resumen, completo = "", espacio = "";
            if (texto.Length > 25)
            {
                stop = texto.Length - 25;
                resumen = texto.Remove(25, stop);
            }
            else
            {
                resumen = texto;
            }
            completo = resumen;
            int numEspacios = maxCaracter - (resumen.Length + total.Length);
            for (int i = 0; i < numEspacios; i++)
            {
                espacio += " ";
            }
            completo += espacio + total;
            lineas.AppendLine(completo);
        }

        public void AgregarArticulo(String articulo, int cant, string precio)
        {
            String elemento = "", espacios = "";
            bool flag = false;
            int numEspacios = 5;
            if (articulo.Length > 20)
            {
                //Coloca cantidades a la derecha
                espacios = "";
                for (int i = 0; i < numEspacios; i++)
                {
                    espacios = " ";
                }
                elemento += espacios + cant.ToString();
                //Coloca precios a la derecha
                espacios = "";
                for (int i = 0; i < numEspacios; i++)
                {
                    espacios = " ";
                }
                elemento  += espacios + precio.ToString();
                int caracterActual = 0;
                for (int i = articulo.Length; i > 20; i -= 20)
                {
                    if (flag)
                    {
                        lineas.AppendLine(articulo.Substring(caracterActual, 20));
                    }
                    else
                    {
                        lineas.AppendLine(articulo.Substring(caracterActual, 20) + elemento);
                        flag = true;
                    }
                }
                lineas.AppendLine(articulo.Substring(caracterActual, articulo.Length - caracterActual));
            }
            else
            {
                for (int i = 0; i < (20 - articulo.Length); i++)
                {
                    espacios += " "; //Llena con espacios la diferencia entre 20 y la longitud de la cadena de articulo
                }
                elemento += articulo + espacios;
                //Colca cantidades a la derecha
                espacios = "";
                for (int i = 0; i < numEspacios; i++)
                {
                    espacios += " "; //Agrega espacios para poner el valor de la cantidad
                }
                elemento += espacios + cant.ToString();
                //Coloca precio a la derecha
                espacios = "";
                for (int i = 0; i < numEspacios; i++)
                {
                    espacios += " "; //Agrega espacios para poner el valor de la cantidad
                }
                elemento += espacios + cant.ToString();
                lineas.AppendLine(elemento);
            }
        }

        public void CortaTicket()
        {
            lineas.AppendLine("\x1B" + "m"); // Caracteres de corte
            lineas.AppendLine("\x1B" + "d" + "\x09"); // Avanza nueve reglones
        }

        public void AbreCajon()
        {
            lineas.AppendLine("\x1B" + "p" + "\x00" + "\x0F" + "\x96"); // Caracteres de apertura de cajon
        }

        public void ImprimirTicket (String impresora)
        {
            RawPrinterHelper.SendStringToPrinter(impresora, lineas.ToString());
            lineas.Clear();
        }


    }
}
