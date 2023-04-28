using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Users.Models
{
    public class TUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }
        public string NID { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
        public string IdUser { get; set; }
        public byte [] Image { get; set;  }
    }
}
