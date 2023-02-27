using Microsoft.Identity.Client;

namespace FactuLib.Areas.Cajas.Models
{
    public class InputModelCierre : InputModelApertura
    {

        public float TotalVentas { get; set; } = 0;

        public float TotalVentasCuentasBancarias { get; set; } = 0;

        public float TotalComprasCuentasBancarias { get; set; } = 0;

        public float TotalCompras { get; set; } = 0;

        public float TotalVentasCredito { get; set;} = 0;

        public float TotalComprasCredito { get; set;} = 0;

        public float MontoCajasApertura { get; set; } = 0;

        public float MontoCuentasApertura { get; set;  } = 0;
    }
}
