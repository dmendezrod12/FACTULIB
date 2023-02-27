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
}