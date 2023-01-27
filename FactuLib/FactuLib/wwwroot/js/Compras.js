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
        } else {
            document.getElementById("labelCompra_Deuda").innerHTML = "";
        }
        let deudas = numberDecimales(deuda);
        document.getElementById("labelCompra_Deudas").innerHTML = deudas.replace('-', '');
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
                        document.getElementById("InputPrecio").value = data.precio_Costo;
                    });
                } else {
                    
                    document.getElementById("ExisteProducto").innerHTML = "No existe codigo de producto";
                    document.getElementById("InputNombre").value = "";
                    document.getElementById("InputDescripcion").value = "";
                    document.getElementById("InputPrecio").value = "";
                }
                
            }
        });
    }

}