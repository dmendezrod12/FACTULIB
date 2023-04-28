using System;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Clientes.Models
{
    public class TCreditoClientes
    {
        [Key]
        public int idDeuda { get; set; }

        public Decimal Deuda { get; set; }

        public Decimal Mensual { get; set; }

        public Decimal Cambio { get; set; }

        public Decimal UltimoPago { get; set; }

        public DateTime FechaPago { get; set; }

        public Decimal DeudaActual { get; set; }

        public DateTime FechaDeuda { get; set; }

        public string Ticket { get; set; }
        public DateTime FechaLimite { get; set; }

        //public int TClientsCedula { get; set; }

        public long CedulaCliente { get; set; }

        public TClientes TClients { get; set; }
    }
}
