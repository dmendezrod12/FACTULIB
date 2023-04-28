class AperturaCaja extends UploadPicture {
    calculaTotal() {
        var CantidadCinco = parseInt(document.getElementById("inputMonCinco").value);
        var CantidadDiez = parseInt(document.getElementById("inputMonDiez").value);
        var CantidadVeintiCinco = parseInt(document.getElementById("inputMonVeinticinco").value);
        var CantidadCincuenta = parseInt(document.getElementById("inputMonCincuenta").value);
        var CantidadCien = parseInt(document.getElementById("inputMonCien").value);
        var CantidadQuinientos = parseInt(document.getElementById("inputMonQuinientos").value);
        var CantidadMil = parseInt(document.getElementById("inputBillMil").value);
        var CantidadDosMil = parseInt(document.getElementById("inputBillDosMil").value);
        var CantidadCincoMil = parseInt(document.getElementById("inputBillCincoMil").value);
        var CantidadDiezMil = parseInt(document.getElementById("inputBillDiezMil").value);
        var CantidadVeinteMil = parseInt(document.getElementById("inputBillVeinteMil").value);
        var CantidadCincuentaMil = parseInt(document.getElementById("inputBillCincuentaMil").value);

        var montoTotal = (CantidadCinco * 5) + (CantidadDiez * 10) + (CantidadVeintiCinco * 25) + (CantidadCincuenta * 50) + (CantidadCien * 100) +
            (CantidadQuinientos * 500) + (CantidadMil * 1000) + (CantidadDosMil * 2000) + (CantidadCincoMil * 5000) + (CantidadDiezMil * 10000) +
            (CantidadVeinteMil * 20000) + (CantidadCincuentaMil * 50000);

        document.getElementById("label_Dinero_En_Caja").innerHTML = numberDecimales(montoTotal);
    }

    dineroCuentas() {
        var cantCuentas = document.getElementById("inputDineroCuentas").value;
        document.getElementById("label_Dinero_En_Cuentas").innerHTML = numberDecimales(cantCuentas);
    }

    faltanteCajas() {
        var resultado = 0;

        var CantidadCinco = parseInt(document.getElementById("inputMonCinco").value);
        var CantidadDiez = parseInt(document.getElementById("inputMonDiez").value);
        var CantidadVeintiCinco = parseInt(document.getElementById("inputMonVeinticinco").value);
        var CantidadCincuenta = parseInt(document.getElementById("inputMonCincuenta").value);
        var CantidadCien = parseInt(document.getElementById("inputMonCien").value);
        var CantidadQuinientos = parseInt(document.getElementById("inputMonQuinientos").value);
        var CantidadMil = parseInt(document.getElementById("inputBillMil").value);
        var CantidadDosMil = parseInt(document.getElementById("inputBillDosMil").value);
        var CantidadCincoMil = parseInt(document.getElementById("inputBillCincoMil").value);
        var CantidadDiezMil = parseInt(document.getElementById("inputBillDiezMil").value);
        var CantidadVeinteMil = parseInt(document.getElementById("inputBillVeinteMil").value);
        var CantidadCincuentaMil = parseInt(document.getElementById("inputBillCincuentaMil").value);
        var montoCajasInicial = parseFloat(document.getElementById("inputDineroApertura").value);
        var montoDineroRecibidoVentas = parseFloat(document.getElementById("inputDineroRecibidoVentas").value);
        var montoDineroRecibidoCompras = parseFloat(document.getElementById("inputDineroRecibidoCompras").value);
        var montoCambiosVentas = parseFloat(document.getElementById("inputCambioVentas").value);
        var montoCambiosCompras = parseFloat(document.getElementById("inputCambioCompras").value);
        var montoCajasFinal = (CantidadCinco * 5) + (CantidadDiez * 10) + (CantidadVeintiCinco * 25) + (CantidadCincuenta * 50) + (CantidadCien * 100) +
            (CantidadQuinientos * 500) + (CantidadMil * 1000) + (CantidadDosMil * 2000) + (CantidadCincoMil * 5000) + (CantidadDiezMil * 10000) +
            (CantidadVeinteMil * 20000) + (CantidadCincuentaMil * 50000);
        var dineroSaliente = montoDineroRecibidoCompras + montoCambiosVentas;
        var dineroEntrante = montoCajasInicial + montoDineroRecibidoVentas + montoCambiosCompras;
        var arqueo = dineroEntrante - dineroSaliente;
        resultado = montoCajasFinal - arqueo;

        if (resultado < 0) {

            document.getElementById("lblFaltanteCajas").innerHTML = numberDecimales(resultado);
        }
        else {
            document.getElementById("lblFaltanteCajas").innerHTML = 0.00;
        }

        if (resultado>0) {
            document.getElementById("lblSobranteCajas").innerHTML = numberDecimales(resultado);
        } else {
            document.getElementById("lblSobranteCajas").innerHTML = 0.00;
        }

        var montoCuentasInicial = parseFloat(document.getElementById("inputMontoCuentasApertura").value);
        var montoCuentasFinal = parseFloat(document.getElementById("inputDineroCuentas").value);
        var montoVentasCuentas = parseFloat(document.getElementById("inputTotalVentasCuentas").value);
        var montoComprasCuentas = parseFloat(document.getElementById("inputTotalComprasCuentas").value);

        var dineroSaliente = montoComprasCuentas;
        var dineroEntrante = montoCuentasInicial + montoVentasCuentas;
        var arqueo = dineroEntrante - dineroSaliente;
        var resultado2 = montoCuentasFinal - arqueo;

        if (resultado2 < 0) {

            document.getElementById("lblFaltanteCuentas").innerHTML = numberDecimales(resultado2);
        }
        else {
            document.getElementById("lblFaltanteCuentas").innerHTML = 0.00;
        }

        if (resultado2 > 0) {
            document.getElementById("lblSobranteCuentas").innerHTML = numberDecimales(resultado2);
        } else {
            document.getElementById("lblSobranteCuentas").innerHTML = 0.00;
        }

    }
}