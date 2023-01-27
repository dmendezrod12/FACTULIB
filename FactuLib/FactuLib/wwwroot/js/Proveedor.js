class Proveedor extends UploadPicture {
    SetSection(value) {
        switch (value) {
            case 1:
                document.getElementById('inlineRadio1').checked = true;
                document.getElementById('inlineRadio1').disabled = false;
                localStorage.setItem("section", value);
                this.Restore();
                break;
        }
    }

    Pagos(event, input) {
        var tempValue;
        var key = window.Event ? event.which : event.keycode;
        var chark = String.fromCharCode(key);
        if (input == null) {
            tempValue = document.getElementById("PagoProveedor").value;
        } else {
            tempValue = input.value + chark;
        }
        var pago1 = parseFloat(tempValue);
        var section = parseInt(localStorage.getItem("section"));
        switch (section) {

            case 1:
                var value1 = document.getElementById("Mensual").value;
                var mensual = parseFloat(value1);
                var value2 = document.getElementById("DeudaActual").value;
                var deuda = parseFloat(value2);
                if (deuda > 0) {
                    if (pago1 >= mensual) {
                        if (pago1>mensual) {
                            let cambio = pago1 - mensual;
                            let value = "El cambio para el sistema es: " + numberDecimales(cambio);
                            document.getElementById("pagoMensaje").innerHTML = value; 
                        }
                        if (pago1 == mensual) {
                            value = "";
                            document.getElementById("pagoMensaje").innerHTML = value;
                        }
                        $('#pago').attr("disabled", false);
                    } else {
                        $("#pago").attr("disabled", true);
                        let importe = mensual - pago1;
                        let value = "El importe faltante es: " + numberDecimales(importe);
                        document.getElementById("pagoMensaje").innerHTML = value;
                    }
                } else {
                    var value = "El sistema no contiene deuda con el proveedor";
                    document.getElementById("pagoMensaje").innerHTML = value; 

                }

                break;
        }
    }

    Restore() {
        document.getElementById("PagoProveedor").value = "";
        document.getElementById("pagoMensaje").innerHTML = "";
        $('#pago').attr("disabled", true);
    }
}