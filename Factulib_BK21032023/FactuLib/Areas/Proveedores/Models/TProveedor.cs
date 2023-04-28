using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Proveedores.Models
{
    public class TProveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        public long Ced_Jur { get; set; }

        public string Nombre_Proveedor { get; set; }

        public DateTime Fecha { get; set; }

        public byte[] Imagen { get; set; }

        public bool Enabled { get; set; }

    }
}
