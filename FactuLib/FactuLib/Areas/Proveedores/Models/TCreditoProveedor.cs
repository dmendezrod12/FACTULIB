using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Proveedores.Models
{
    public class TCreditoProveedor
    {
        [Key]
        public int idCredito { get; set; }

        public Decimal Deuda { get; set; }

        public Decimal Mensual { get; set; }

        public Decimal Cambio { get; set; }

        public Decimal UltimoPago { get; set; }

        public DateTime FechaPago { get; set; }

        public Decimal DeudaActual { get; set; }

        public DateTime FechaDeuda { get; set; }

        public string Ticket { get; set; }
        public DateTime FechaLimite { get; set; }

        [ForeignKey("IdProveedor")]
        public TProveedor TProveedor { get; set; }

    }
}
