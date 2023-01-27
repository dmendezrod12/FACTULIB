using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactuLib.Areas.Clientes.Models
{
    public class THistoricoPagosClientes
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

        public string Ticket { get; set; }

        public string IdUser { get; set; }

        public string User { get; set;  }

        [ForeignKey("CedulaCliente")]
        public long CedulaCliente { get; set; }
    }
}
