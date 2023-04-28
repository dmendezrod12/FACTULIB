using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FactuLib.Areas.Proveedores.Models
{
    public class THistoricoPagosProveedor
    {
        [Key]
        public int IdPago { get; set; }

        [Display(Name = "Deuda Inicial")]

        public decimal Deuda { get; set; }

        public decimal Cambio { get; set; }

        [Display(Name = "Pago Recibido")]
        public decimal Pago { get; set; }

        public DateTime Fecha { get; set; }

        public Decimal DeudaActual { get; set; }

        public DateTime FechaLimite { get; set; }

        public DateTime FechaDeuda { get; set; }

        public Decimal Mensual { get; set; }

        public Decimal DeudaAnterior { get; set; }

        public string Ticket { get; set; }

        public string IdUser { get; set; }

        public string User { get; set; }

        [ForeignKey("idProveedor")]

        public TProveedor proveedor { get; set; }
    }
}
