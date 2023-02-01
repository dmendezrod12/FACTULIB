// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var principal = new Principal();

/*Codigo de usuarios*/

var user = new User();
var imageUser = (evt) => {
    user.archivo(evt, "imageUser");
}

/*Codigo de clientes*/

var clientes = new Clientes();
var imageCliente = (evt) => {
    clientes.archivo(evt, "imageCliente");
}
$().ready(() => {
    let URLactual = window.location.pathname;
    principal.userLink(URLactual);

});

/*Codigo de Compras */

var compras = new Compras();
var imageShopping = (evt) => {
    compras.archivo(evt,"imageShopping")
}
/*Codigo de Proveedor*/

var proveedor = new Proveedor();
var imageProveedor = (evt) => {
    proveedor.archivo(evt, "imageProveedor");
}

/*Codigo de Productos*/

var productos = new Productos();
var imageProducto = (evt) => {
    productos.archivo(evt, "imageProducto");
}
$().ready(() => {
    let URLactual = window.location.pathname;
    principal.userLink(URLactual);

    /*Codigo de Proveeodr*/

    

    /*Codigo de Compras */

   

});