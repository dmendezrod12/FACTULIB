
class Principal {
    userLink(URLactual) {
        var proveedor = new Proveedor();
        var compras = new Compras();
        let url = "";
        let cadena = URLactual.split("/");
        for (var i = 0; i < cadena.length; i++) {
            if (cadena[i] != "Index") {
                url += cadena[i];
            }
        }
        switch (url) {
            case "UsersRegister":
                document.getElementById('files').addEventListener('change', imageUser, false);
                break;
            case "ClientesRegister":
                document.getElementById('files').addEventListener('change', imageCliente, false);
                $(document).ready(function () {
                    $("#ddlTipoCedula").change(function () {
                        var opcion = document.getElementById("ddlTipoCedula").value;
                        switch (opcion) {
                            case "1":
                                document.getElementById('CampoCedula').style.display = 'block';
                                document.getElementById('CampoCedulaJuridica').style.display = 'none';
                                document.getElementById('CampoCedulaResidencia').style.display = 'none';
                                document.getElementById('CampoCedulaNITE').style.display = 'none';
                                document.getElementById("CedulaAyuda").innerHTML =
                                    "<small>El numero de cédula tiene que contener 9 digitos númericos.</small>";
                                document.getElementById("InputCedula").value = "0";
                                document.getElementById("InputCedulaResidencia").value = "999999999999";
                                document.getElementById("InputCedulaJuridica").value = "9999999999";
                                document.getElementById("InputCedulaNITE").value = "9999999999";
                                break;
                            case "2":
                                document.getElementById('CampoCedula').style.display = 'none';
                                document.getElementById('CampoCedulaResidencia').style.display = 'none';
                                document.getElementById('CampoCedulaNITE').style.display = 'none';
                                document.getElementById('CampoCedulaJuridica').style.display = 'block';
                                document.getElementById("CedulaAyuda").innerHTML =
                                    "<small>El numero de cédula Juridica tiene que contener 10 digitos númericos.</small>";
                                document.getElementById("InputCedulaJuridica").value = "0";
                                document.getElementById("InputCedula").value = "999999999";
                                document.getElementById("InputCedulaResidencia").value = "999999999999";
                                document.getElementById("InputCedulaNITE").value = "9999999999";
                                break;
                            case "3":
                                document.getElementById('CampoCedula').style.display = 'none';
                                document.getElementById('CampoCedulaJuridica').style.display = 'none';
                                document.getElementById('CampoCedulaNITE').style.display = 'none';
                                document.getElementById('CampoCedulaResidencia').style.display = 'block';
                                document.getElementById("CedulaAyuda").innerHTML =
                                    "<small>El numero de cédula DIMEX tiene que contener 12 digitos númericos.</small>";
                                document.getElementById("InputCedulaResidencia").value = "0";
                                document.getElementById("InputCedula").value = "999999999";
                                document.getElementById("InputCedulaJuridica").value = "9999999999";
                                document.getElementById("InputCedulaNITE").value = "9999999999";
                                break;
                            case "4":
                                document.getElementById('CampoCedula').style.display = 'none';
                                document.getElementById('CampoCedulaJuridica').style.display = 'none';
                                document.getElementById('CampoCedulaResidencia').style.display = 'none';
                                document.getElementById('CampoCedulaNITE').style.display = 'block';
                                document.getElementById("CedulaAyuda").innerHTML =
                                    "<small>El numero de cédula NITE tiene que contener 10 digitos númericos.</small>";
                                document.getElementById("InputCedulaNITE").value = "0";
                                document.getElementById("InputCedula").value = "999999999";
                                document.getElementById("InputCedulaJuridica").value = "9999999999";
                                document.getElementById("InputCedulaResidencia").value = "999999999999";
                                break;
                        }
                    });
                });
                break;
            case "ProveedoresRegistrar":
                document.getElementById('files').addEventListener('change', imageProveedor, false);
                break;
            case "ProveedoresCredito":
                proveedor.SetSection(1);
                $('#PagoProveedor').keyup((e) => {
                    var key = e.which || e.keyCode || e.charCode;
                    if (key == 8) {
                        proveedor.Pagos(e, null);
                    }
                    return true;
                });
            case "ComprasAgregarCompra":
                $("#Input_Cantidad").change((e) => {
                    compras.purchaseAmount()
                });
                document.getElementById('files').addEventListener('change', imageShopping, false);
                compras.Restore();
                break;
            case "ProductosAgregarProducto":
                document.getElementById('files').addEventListener('change', imageProducto, false);
                break;
                
        }
    }
}