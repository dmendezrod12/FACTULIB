using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Clientes.Models
{
    public class TClientes
    {
        [Key]

        public int IdCliente { get; set; }

        public long Cedula { get; set; }

        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        //public string Telefono { get; set; }

        public DateTime Fecha { get; set; }

        public bool Credito { get; set; }

        public byte[] Imagen { get; set; }

        public bool Enabled { get; set; }

       // public TCorreosClientes correosClientes { get; set; }
        
       // public List<TCreditoClientes> TCreditoClientes { get; set; }
    }
}
