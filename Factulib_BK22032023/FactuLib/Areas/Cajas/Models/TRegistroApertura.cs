using FactuLib.Areas.Users.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Cajas.Models
{
    public class TRegistroApertura
    {
        [Key]
        public int Id_Apertura { get; set; }

        [ForeignKey("Id_Usuario")]
        public TUser TUser { get; set; }

        public DateTime Fecha_Apertura { get; set; }

        public float Dinero_Cajas { get; set; }

        public float Dinero_Cuentas { get; set; }

        public bool Estado { get; set; }

    }
}
