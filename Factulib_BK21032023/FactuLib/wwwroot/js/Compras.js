class Compras extends UploadPicture {

    Restore() {
        document.getElementById("Input_Pagos").val = 0.0;
        this.purchaseAmount();
    }

    purchaseAmount() {
        var Cantidad = document.getElementById("Input_Cantidad").value;

        var Precio = document.getElementById("InputPrecio").value;

        var Descuento = document.getElementById("Input_Descuento").value;

        var Amount = Precio * Cantidad;

        document.getElementById("labelCompra_Importe").innerHTML = numberDecimales(Amount);

        var Descuento = Amount * (Descuento / 100);

        var Impuesto = Amount * 0.13;

        var TotalAmount = Amount + Descuento + Impuesto;

        document.getElementById("labelCompra_Descuento").innerHTML = numberDecimales(Descuento);

        document.getElementById("labelCompra_Impuesto").innerHTML = numberDecimales(Impuesto);

        document.getElementById("labelCompra_Neto").innerHTML = numberDecimales(TotalAmount);

    }

    Pagos(monto) {
        var pagos = document.getElementById("Input_Pagos").value;
        let deuda = monto - pagos;
        if (monto < pagos) {
            document.getElementById("labelCompra_Deuda").innerHTML = "Cambio para el sistema";
            document.getElementById("checkContado").disabled = false;
            document.getElementById("checkCredito").disabled = true;
        } else {
            document.getElementById("labelCompra_Deuda").innerHTML = "Monto de Deuda";
            document.getElementById("checkContado").disabled = true;
            document.getElementById("checkCredito").disabled = false;
        }
        let deudas = numberDecimales(deuda);
        document.getElementById("labelCompra_Deudas").innerHTML = deudas.replace('-', '');
    }

    CheckCredito() {
        if (document.getElementById("checkCredito").checked) {
            document.getElementById("checkContado").disabled = true;
            document.getElementById("Input_Pagos").value = 0;
            document.getElementById("Input_Pagos").disabled = true;
        } else {
            document.getElementById("checkContado").disabled = false;
            document.getElementById("Input_Pagos").disabled = false;
        }
    }

    CheckContado() {
        if (document.getElementById("checkContado").checked) {
            document.getElementById("checkCredito").disabled = true;
            document.getElementById("checkTransferencia").disabled = false;
            document.getElementById("checkTarjeta").disabled = false;
            document.getElementById("checkEfectivo").disabled = false;
        } else {
            document.getElementById("checkCredito").disabled = false;
            document.getElementById("checkTransferencia").disabled = true;
            document.getElementById("checkTarjeta").disabled = true;
            document.getElementById("checkEfectivo").disabled = true;
        }
    }

    CheckEfectivo() {
        if (document.getElementById("checkEfectivo").checked) {            
            document.getElementById("checkTransferencia").disabled = true;
            document.getElementById("checkTarjeta").disabled = true;
            document.getElementById("checkEfectivo").disabled = false;
        } else {
            document.getElementById("checkTransferencia").disabled = false;
            document.getElementById("checkTarjeta").disabled = false;
            document.getElementById("checkEfectivo").disabled = false;
        }
    }


    CheckTarjeta() {
        if (document.getElementById("checkTarjeta").checked) {
            document.getElementById("checkTransferencia").disabled = true;
            document.getElementById("checkTarjeta").disabled = false;
            document.getElementById("checkEfectivo").disabled = true;
        } else {
            document.getElementById("checkTransferencia").disabled = false;
            document.getElementById("checkTarjeta").disabled = false;
            document.getElementById("checkEfectivo").disabled = false;
        }
    }

    CheckTransferencia() {
        if (document.getElementById("checkTransferencia").checked) {
            document.getElementById("checkTransferencia").disabled = false;
            document.getElementById("checkTarjeta").disabled = true;
            document.getElementById("checkEfectivo").disabled = true;
        } else {
            document.getElementById("checkTransferencia").disabled = false;
            document.getElementById("checkTarjeta").disabled = false;
            document.getElementById("checkEfectivo").disabled = false;
        }
    }


    BusquedaProducto() {
        var idProducto = document.getElementById("InputIdProducto").value;
        document.getElementById("ExisteProducto").innerHTML = "";
        let count = 0;
        $.get("/Compras/GetProducto", { idProducto: idProducto }, function (data) {
            if (data != null) {
                count = 0;
                document.getElementById("InputNombre").value = "";

                $.each(data, function (index, row) {
                    if (index == "id_Producto") {
                        count++;
                    }  
                });
                
                if (count == 1) {
                    // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                    $.each(data, function (index, row) {
                        document.getElementById("InputNombre").value = data.nombre_Producto;
                        document.getElementById("InputDescripcion").value = data.descripcion_Producto;
                        document.getElementById("InputPrecio").value = data.precioProductoJS;
                        document.getElementById("Input_Descuento").value = data.descuento_Producto;
                        document.getElementById("imgProductoCompra").src = data.base64Imagen;
                    });
                } else {
                    
                    document.getElementById("ExisteProducto").innerHTML = "No existe codigo de producto";
                    document.getElementById("InputNombre").value = "";
                    document.getElementById("InputDescripcion").value = "";
                    document.getElementById("InputPrecio").value = "";
                    document.getElementById("imgProductoCompra").src = "/images/images/agregarCompra.png";
                }
                
            }
        });
    }

}