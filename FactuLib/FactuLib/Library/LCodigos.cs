using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Library
{
    public class LCodigos
    {
        public string codigosTickets(String Codigo)
        {
            String ticket = null;
            if (Codigo.Equals("0000000000"))
            {
                ticket = "0000000001";
            }
            else
            {
                if (Codigo.Equals("9999999999"))
                {
                    ticket = "0000000001";
                }
                else
                {
                    var cod = Convert.ToInt64(Codigo);
                    cod++;
                    ticket = cod.ToString("D10");
                }
            }
            return ticket;
        }
    }
}
