using System;

namespace FactuLib.Areas.Principal.Models
{
    public class PrincipalModel
    {
       public string estadoCaja { get; set; }  

       public DateTime FechaSiste { get; set; } = DateTime.Now;
    }
}
