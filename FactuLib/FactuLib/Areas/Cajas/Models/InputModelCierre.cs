using Microsoft.Identity.Client;
using System;

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

        public float MontoCajasCierre { get; set; } = 0;

        public float MontoCuentasCierre { get; set; } = 0;

        public float MontoFaltanteCuentas { get; set; } = 0;

        public float MontoFaltanteCajas { get; set; } = 0;

        public float MontoSobranteCuentas { get; set; } = 0;

        public float MontoSobranteCajas { get; set; } = 0;

        public DateTime horaInicio { get; set; } = DateTime.Now.Date;

        public DateTime horaFinal { get; set; } = DateTime.Now.Date;

    }
}
