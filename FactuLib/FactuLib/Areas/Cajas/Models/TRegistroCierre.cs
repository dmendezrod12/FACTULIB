using FactuLib.Areas.Users.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Cajas.Models
{
    public class TRegistroCierre
    {
        [Key]
        public int Id_Cierre { get; set; }

        [ForeignKey("Id_Usuario")]
        public TUser TUser { get; set; }

        [ForeignKey("Id_Apertura")]

        public TRegistroApertura TRegistroApertura { get; set; }

        public DateTime Fecha_Cierre { get; set; }

        public float Dinero_Cajas { get; set; }

        public float Dinero_Cuentas { get; set; }

        public float Total_Ventas_Efectivo { get; set; }

        public float Total_Ventas_Cuentas { get; set; }

        public float Total_Ventas_Credito { get; set; }

        public float Total_Compras_Efectivo { get; set; }

        public float Total_Compras_Credito { get; set; }

        public float Total_Compras_Cuentas { get; set; }

        public float Monto_Faltante_Cajas { get; set; }

        public float Monto_Faltante_Cuentas { get; set; }

        public float Monto_Sobrante_Cajas { get; set; }

        public float Monto_Sobrante_Cuentas { get; set; }
    }
}
