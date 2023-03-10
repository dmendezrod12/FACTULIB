using Azure;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Ventas.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Ventas.Pages.Account
{
    [Authorize]
    [Area("Ventas")]
    public class AgregarVentaModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private UploadImage _uploadImage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LVentas _ventas;
        private LApertura _apertura;
        private LProducto _producto;
        private LSendMails _correo;
        private static InputModelVentas _temporalVentas;
        private static byte[] imageByte = null;
        private static bool ventaIniciada = false;
        public int _idPagina;
        public string Moneda = "¢";
        public float monto, montoDescuento, montoImpuesto, montoBruto = 0;
        public static string _clienteSeleccionado;
        public static string _nombreCliente = "";

        public AgregarVentaModel(
           ApplicationDbContext context,
           IWebHostEnvironment environment,
           UserManager<IdentityUser> userManager)
        {
            _uploadImage = new UploadImage();
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _ventas = new LVentas(context);
            _apertura = new LApertura(context);
            _producto = new LProducto(context);
            _correo = new LSendMails();
            Moneda = "¢";
        }


        public void OnGet(int id, int IdTemporal, string nombreCliente, int idProducto, string search, bool error)
        {
            _idPagina = id;
            monto = _ventas.getMontoTotal();
            montoBruto = _ventas.getMontoBruto();
            montoDescuento = _ventas.getMontoDescuentos();
            montoImpuesto = _ventas.getMontoImpuestos();

            if (_dataInput != null)
            {
                Input = _dataInput;
                Input.AvatarImage = null;
                Input.Image = _dataInput.Image;
                Input.Temporal_Ventas = _ventas.Get_Temporal_Ventas(_idPagina, 5, search, Request);
                Input.Lista_Clientes = _ventas.GetClientesLista(_idPagina, 5, search, Request);
                Input.Lista_Productos = _ventas.GetProductosLista(_idPagina, 5, search, Request);

                if (error == true)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }

            }

            if (_dataInput2 != null)
            {
                Input = _dataInput2;
                if (error == true)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }
            }
            else
            {
                _temporalVentas = _ventas.getVentas(IdTemporal);
                Input = new InputModel
                {
                    Temporal_Ventas = _ventas.Get_Temporal_Ventas(_idPagina, 5, search, Request),
                    Lista_Clientes = _ventas.GetClientesLista(_idPagina, 5, search, Request),
                    Lista_Productos = _ventas.GetProductosLista(_idPagina, 5, search, Request),
                    idTempVentas = _temporalVentas.idTempVentas,
                    Nombre = _temporalVentas.Nombre,
                    Descripcion = _temporalVentas.Descripcion,
                    Cantidad = _temporalVentas.Cantidad,
                    Precio = _temporalVentas.Precio,
                    TotalBruto = _temporalVentas.TotalBruto,
                    Descuento = _temporalVentas.Descuento,
                    Image = _temporalVentas.Image,
                    Date = _temporalVentas.Date,
                };

                if (error == true)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }

                if (Input.idTempVentas != 0)
                {
                    var producto = _context.TProducto.Where(p => p.Nombre_Producto.Replace(" ", "").Equals(Input.Nombre.Replace(" ", ""))).ToList().Last();
                    Input.TProducto = producto;
                    Input.InputIdProducto = producto.Id_Producto;
                }
                if (_temporalVentas.TCliente != null)
                {
                    _temporalVentas.TCliente = _temporalVentas.TCliente;
                    Input.Cliente = _temporalVentas.TCliente.Nombre;
                }

                if (_nombreCliente != "")
                {
                    Input.Cliente = _nombreCliente;
                }

                if (nombreCliente != null)
                {
                    _nombreCliente = nombreCliente;
                    Input.Cliente = _nombreCliente;
                }

                if (idProducto > 0)
                {
                    var producto = Input.Lista_Productos.List.Where(p => p.Id_Producto.Equals(idProducto)).ToList().Last();
                    Input.Nombre = producto.Nombre_Producto;
                    Input.Descripcion = producto.Descripcion_Producto;
                    Input.Precio = (float)producto.Precio_Costo;
                    Input.Image = producto.Imagen;
                    Input.InputIdProducto = producto.Id_Producto;
                    Input.Descuento = producto.Descuento_Producto;
                }

                if (_temporalVentas.TUser != null)
                {
                    Input.TUser = _temporalVentas.TUser;
                    Input.User = _temporalVentas.TUser.Name;
                }
            }

        }

        public async Task<IActionResult> OnPost(int delete, String NombreCliente, int SelCliente, int value, int SelProd, int IdProd)
        {
            if (SelCliente == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Ventas/AgregarVenta?area=Ventas&nombreCliente=" + NombreCliente);
            }

            if (SelProd == 1)
            {
                return Redirect("/Ventas/AgregarVenta?area=Ventas&idProducto=" + IdProd);
            }
            if (delete > 0)
            {
                await DeleteAsync(delete);
            }
            else
            {
                if (value == 1)
                {
                    if (!_apertura.ValidaApertura(_userManager.GetUserId(User)))
                    {
                        if (await VentasAsync())
                        {
                            return Redirect("/Ventas/AgregarVenta?area=Ventas");
                        }
                        else
                        {
                            return Redirect("/Ventas/AgregarVenta?area=VGentas&error=true");
                        }
                    }
                    else
                    {
                        return Redirect("/Cajas/Apertura_Caja?area=Cajas");
                    }
                    
                }
                else
                {
                    if (_temporalVentas.idTempVentas == 0)
                    {
                        if (!_apertura.ValidaApertura(_userManager.GetUserId(User)))
                        {
                            if (await SaveAsync())
                            {
                                return Redirect("/Ventas/AgregarVenta?area=Ventas");
                            }
                            else
                            {
                                return Redirect("/Ventas/AgregarVenta?area=Ventas&error=true");
                            }
                        }
                        else
                        {
                            return Redirect("/Cajas/Apertura_Caja?area=Cajas");
                        }       
                    }
                    else
                    {
                        await EditAsync();
                    }
                }
            }
            return Redirect("/Ventas/AgregarVenta?area=Ventas");
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelVentas
        {
            public string User { get; set; }

            public string NombreCliente { get; set; }

            public string CedCliente { get; set; }

            public int IdProducto { get; set; }

            public int InputIdProducto { get; set; }

            public int NombreProducto { get; set; }

            public int CantidadProducto { get; set; }
            public string Moneda { get; set; } = "¢";

            public IFormFile AvatarImage { get; set; }

            public string ErrorMessage { get; set; }

            public List<TTemporalVentas> Ventas { get; set; }

            public DataPaginador<InputModelVentas> Temporal_Ventas { get; set; }

            public DataPaginador<TClientes> Lista_Clientes { get; set; }

            public DataPaginador<TProducto> Lista_Productos { get; set; }

        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                imageByte = await _uploadImage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/agregarCompra.png");
                _dataInput.Image = imageByte;
                var cliente = _context.TClientes.Where(p => p.Nombre.Replace(" ", "").Equals(_dataInput.Cliente.Replace(" ", ""))).ToList();
                if (0 < cliente.Count)
                {
                    var dataCliente = cliente.Last();
                    var idUser = _userManager.GetUserId(User);
                    bool clienteInvalido = false;
                    var dataVentas = _context.TTemporalVentas.Where(t => t.TCliente.IdCliente.Equals(dataCliente.IdCliente) && t.TUser.IdUser.Equals(idUser)).ToList();

                    if (dataVentas.Count == 0 && ventaIniciada == true)
                    {
                        clienteInvalido = true;
                    }

                    if (dataVentas.Count == 0 && clienteInvalido == false)
                    {
                        await SaveAsync();         
                    }
                    else
                    {
                        if (clienteInvalido == false)
                        {
                            await SaveAsync();
                        }
                        else
                        {
                            _dataInput.ErrorMessage = "Finalizar la compra del cliente en lista";
                        }
                    }
                    async Task SaveAsync()
                    {
                        _clienteSeleccionado = _dataInput.Cliente;
                        var strategy = _context.Database.CreateExecutionStrategy();
                        var userData = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList().Last();
                        var producto = _context.TProducto.Where(p => p.Id_Producto.Equals(_dataInput.InputIdProducto)).ToList().Last();
                        if (producto.Cantidad_Producto > _dataInput.Cantidad)
                        {
                            await strategy.ExecuteAsync(async () =>
                            {
                                using (var transaction = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        float monto = _dataInput.Precio * _dataInput.Cantidad;
                                        float descuento = monto * (_dataInput.Descuento / 100);
                                        float impuesto = (float)(monto * 0.13);
                                        float montoNeto = monto + descuento + impuesto;
                                        var ventas = new TTemporalVentas
                                        {
                                            TotalBruto = monto,
                                            TotalDescuentos = descuento,
                                            TotalImpuestos = impuesto,
                                            TotalNeto = montoNeto,
                                            Cantidad_Venta = _dataInput.Cantidad,
                                            TCliente = dataCliente,
                                            TProducto = producto,
                                            TUser = userData,
                                            Date = DateTime.Now
                                        };

                                        await _context.AddAsync(ventas);
                                        _context.SaveChanges();
                                        transaction.Commit();
                                        _dataInput = null;
                                        valor = true;
                                        ventaIniciada = true;
                                    }
                                    catch (Exception ex)
                                    {

                                        _dataInput.ErrorMessage = ex.Message;
                                        transaction.Rollback();
                                        valor = false;

                                        if (dataVentas.Count.Equals(0) && ventaIniciada == true)
                                        {
                                            ventaIniciada = false;
                                        }
                                    }
                                }
                            });
                        }
                        else
                        {
                            _dataInput.ErrorMessage = "No existen productos disponibles, la cantidad disponible es de: " + producto.Cantidad_Producto;
                        }
                    }
                }
                else
                {
                    _dataInput.ErrorMessage = "El cliente no esta registrado";
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _dataInput.ErrorMessage += error.ErrorMessage;
                    }
                }
                valor = false;
            }
            return valor;
        }

        private async Task<bool> EditAsync()
        {
            var valor = false;
            _dataInput = Input;
            var producto = _context.TProducto.Where(p => p.Id_Producto.Equals(_dataInput.InputIdProducto)).ToList().Last();
            List<TTemporalVentas> tempVentas = null;
            if (Input.AvatarImage != null)
            {
                imageByte = await _uploadImage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/agregarCompra.png");
            }
            var cliente = _context.TClientes.Where(u => u.Nombre.Replace(" ", "").Equals(_dataInput.Cliente.Replace(" ", ""))).ToList();

            if (0 < cliente.Count)
            {
                var datosCliente = cliente.Last();
                var idUser = _userManager.GetUserId(User);
                using (var dbContext = new ApplicationDbContext())
                {
                    var clientes = dbContext.TClientes.ToList();
                    var usuarios = dbContext.TUsers.ToList();
                    tempVentas = dbContext.TTemporalVentas.Where(p => p.TCliente.IdCliente.Equals(datosCliente.IdCliente) && p.TUser.IdUser.Equals(idUser)).ToList();
                }
                if (tempVentas[0].TCliente.IdCliente.Equals(datosCliente.IdCliente))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    var usuario = _context.TUsers.Where(u => u.IdUser.Equals(tempVentas[0].TUser.IdUser)).ToList().Last();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var monto = _dataInput.Precio * _dataInput.Cantidad;
                                float descuento = monto * (_dataInput.Descuento / 100);
                                float impuesto = (float)(monto * 0.13);
                                float montoNeto = monto + descuento + impuesto;
                                var ventas = new TTemporalVentas
                                {
                                    idTempVentas = _temporalVentas.idTempVentas,
                                    Date = _temporalVentas.Date,
                                    TotalDescuentos = descuento,
                                    TotalImpuestos = impuesto,
                                    TotalNeto = montoNeto,
                                    TotalBruto = monto,
                                    Cantidad_Venta = _dataInput.Cantidad,
                                    TCliente = datosCliente,
                                    TProducto = producto,
                                    TUser = usuario,
                                };
                                _context.Update(ventas);
                                _context.SaveChanges();
                                transaction.Commit();
                                _dataInput = null;
                                _temporalVentas = null;
                                valor = true;
                            }
                            catch (Exception ex)
                            {

                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                                valor = false;
                            }
                        }
                    });
                }
                else
                {
                    _dataInput.ErrorMessage = "Finalizar la compra de cliente en lista";
                }
            }
            else
            {
                _dataInput.ErrorMessage = "El cliente no esta registrado";
            }
            return valor;
        }

        private async Task DeleteAsync(int id)
        {
            TTemporalVentas datosVentas = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            datosVentas = dbContext.TTemporalVentas.Where(t => t.idTempVentas.Equals(id)).ToList().Last();
                        }
                        _context.Remove(datosVentas);
                        _context.SaveChanges();
                        transaction.Commit();
                        _dataInput = null;
                        _temporalVentas = null;

                    }
                    catch (Exception ex)
                    {

                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
        }

        private async Task<bool> VentasAsync()
        {
            _dataInput = Input; // Variable que almacena los datos que fueron ingresados en el formulario
            var valor = false; // Variable utilizada para dar el resultado de la operación satisfactorio o fallido. 
            var idUser = _userManager.GetUserId(User); // Devuelve el Id del usuario logueado en el sistema
            var userReg = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList().Last(); // Devuelve el registro de usuario logueado al sistema
            var cliente = _context.TClientes.Where(u => u.Nombre.Replace(" ", "").Equals(_clienteSeleccionado.Replace(" ", ""))).ToList().Last(); // Retorna el registro de cliente de la venta
            var productos = _context.TProducto.ToList(); // Retorna el listado de productos registrados en el sistema.
            var clientes = _context.TClientes.ToList(); //Retorna el listado de cliente registrados en el sistema.
            var creditosCliente = _context.TCreditoClientes.ToList(); // Retorna el listado de creditos de clientes registrados en el sistema
            var credito = creditosCliente.Where(c => c.TClients.IdCliente.Equals(cliente.IdCliente)).ToList().Last(); // Retorna el credito del cliente de la venta
            var RegistrosVentas = _context.TRegistroVentas.ToList(); // Retorna los registros de ventas en el sistema
            int numeroTicket = 0; // Numero de ticket asignado a la venta
            
            // Si no existen ventas registradas el numero de ticket sera 1, de lo contrario el numero de ticket sera el ultimo registro +1. 
            if (RegistrosVentas.Count == 0)
            {
                numeroTicket = 1;
            }
            else
            {
                numeroTicket = RegistrosVentas.ToList().Last().Id_RegistroVentas + 1;
            }

            //Trae los datos de la tabla temporal de ventas donde contiene los productos a vender. 
            List<TTemporalVentas> datosTemporal = _context.TTemporalVentas.Where(u => u.TUser.IdUser.Equals(idUser) && u.TCliente.IdCliente.Equals(cliente.IdCliente)).ToList();
            if (datosTemporal.Count > 0)
            {
                var strategy = _context.Database.CreateExecutionStrategy(); // Apertura una strategia de base de datos para abrir una transaccion. 
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction()) // Abre una transacción de base de datos 
                    {
                        try
                        {
                            string _ticket = null; // Declara variable de ticket
                            var dateNow = DateTime.Now; // Declara variable con fecha actual del sistema
                            var metodoPago = 0; // Declara variable de metodo de pago
                            var _cambio = 0.0m; // Declara la variable de cambio al momento de pagar la venta. 
                            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList(); // Declara la variable de usuario
                            var nameUser = $"{user[0].Name} {user[0].Apellido1} {user[0].Apellido2}"; // Declara la variable de nombre de usuario
                            var nameCliente = $"{cliente.Nombre} {cliente.Apellido1} {cliente.Apellido2}"; // Declara el nombre del cliente
                            var monto = _ventas.getMontoTotal(); //Calcula el monto neto de la venta
                            var montoBruto = _ventas.getMontoBruto(); // Calcula el monto bruto de la venta
                            var montoDescuento = _ventas.getMontoDescuentos(); // Calcula el monto descuento de la venta
                            var montoImpuesto = _ventas.getMontoImpuestos(); // Calcula el monto de impuesto de la venta
                            var dataTempo = datosTemporal.Last(); // declara variable con el ultimo registro de la tabla temporal
                            var deuda = monto - Input.Pagos; // Declara el monto de la deuda de la venta en caso de ser a credito.
                            
                            // Si la venta es a contado realiza los siguientes procedimientos
                            if (_dataInput.Contado == true)
                            {
                                //Establece el metodo de pago
                                if (_dataInput.ChkEfectivo == true)
                                {
                                    metodoPago = 1;
                                }
                                if (_dataInput.ChkTarjeta == true)
                                {
                                    metodoPago = 2;
                                }
                                if (_dataInput.ChkTransferencia == true)
                                {
                                    metodoPago = 3;
                                }
                                
                                //Calcula el cambio del sistema
                                _cambio = (decimal)(_dataInput.Pagos - monto);

                                //Declara el objeto de Registro de venta a cargarse en base de datos
                                var RegistroVenta = new TRegistroVentas
                                {
                                    Total_Bruto = montoBruto,
                                    Total_Descuento = montoDescuento,
                                    Total_IVA = montoImpuesto,
                                    Total_Neto = monto,
                                    MetodoPago = metodoPago,
                                    DineroRecibido = _dataInput.Pagos,
                                    CambioCompra = _cambio,
                                    Fecha_Compra = DateTime.Now,
                                    TClientes = cliente,
                                    TUser = userReg,
                                    Estado_Venta = true
                                };

                                //Agrega el objeto en base de datos
                                await _context.AddAsync(RegistroVenta);
                                // Guarda cambios 
                                await _context.SaveChangesAsync();


                                //Agrega todos los productos ingresados en la tabla temporal de ventas, a la venta
                                foreach (var item in datosTemporal)
                                {
                                    var ProductoDetalle = productos.Where(p => p.Id_Producto.Equals(item.TProducto.Id_Producto)).ToList().Last();
                                    ProductoDetalle.Cantidad_Producto = ProductoDetalle.Cantidad_Producto - item.Cantidad_Venta;

                                    //Declara el objeto de detalles de la venta 
                                    var DetalleVenta = new TDetallesVentas
                                    {
                                        Cantidad_Producto = item.Cantidad_Venta,
                                        Monto_Bruto_Detalle = item.TotalBruto,
                                        Descuento_Detalle = item.TotalDescuentos,
                                        Monto_Neto_Detalle = item.TotalNeto,
                                        Monto_Impuesto_Detalle = item.TotalImpuestos,
                                        TRegistroVentas = RegistroVenta,
                                        TProducto = ProductoDetalle
                                    };

                                    //Agrega el objeto en base de datos
                                    await _context.AddAsync(DetalleVenta);
                                    //Guarda cambios
                                    await _context.SaveChangesAsync();

                                    //Elimina Información de la tabla temporal
                                    _context.TTemporalVentas.Where(t => t.idTempVentas.Equals(item.idTempVentas)).ToList().ForEach(d => _context.TTemporalVentas.Remove(d));
                                    await _context.SaveChangesAsync();

                                    if (_producto.compruebaCantidadMinimaProducto(ProductoDetalle.Id_Producto))
                                    {
                                        _correo.SendEmailInventario(ProductoDetalle.Nombre_Producto, ProductoDetalle.Cantidad_Producto, ProductoDetalle.Codigo_Producto);
                                    }
                                }
                                valor = true;
                                //Genera ticket al finalizar la venta
                                GeneraTicket(numeroTicket, nameCliente, nameUser, datosTemporal,montoBruto,montoImpuesto,montoDescuento,monto);
                                //Realiza commit en la transacción para guardar los registros en base de datos
                                transaction.Commit();
                                ventaIniciada = false;
                            }
                            // Si la venta es a credito realiza los siguientes procedimientos
                            if (_dataInput.Credito == true)
                            {
                                
                                //Declara los valores de deuda a ingresar para la venta

                                var deudaActualAnterior = (float)credito.DeudaActual;
                                var deudaNueva = deudaActualAnterior + monto;
                                var deudaActualNueva = deudaNueva;

                                //Declara el objeto de credito de cliente

                                var CreditoCliente = new TCreditoClientes
                                {
                                    //idDeuda = credito.idDeuda,
                                    Deuda = (Decimal)deudaNueva,
                                    Mensual = credito.Mensual,
                                    Cambio = credito.Cambio,
                                    UltimoPago = credito.UltimoPago,
                                    FechaPago = credito.FechaPago,
                                    DeudaActual = (Decimal)deudaActualNueva,
                                    FechaDeuda = credito.FechaDeuda,
                                    Ticket = credito.Ticket,
                                    FechaLimite = credito.FechaLimite,
                                    TClients = cliente,
                                };

                                //Envia a actualizar el objeto en base de datos 
                                _context.Update(CreditoCliente);
                                //Guarda cambios realizados en base de datos
                                _context.SaveChanges();

                                //Declara el objeto de Registro de venta a cargarse en base de datos
                                var RegistroVenta = new TRegistroVentas
                                {
                                    Total_Bruto = montoBruto,
                                    Total_Descuento = montoDescuento,
                                    Total_IVA = montoImpuesto,
                                    Total_Neto = monto,
                                    MetodoPago = 0,
                                    DineroRecibido = 0,
                                    CambioCompra = 0,
                                    Fecha_Compra = DateTime.Now,
                                    TClientes = cliente,
                                    TUser = userReg,
                                    Estado_Venta = true
                                };

                                //Agrega el objeto en base de datos
                                await _context.AddAsync(RegistroVenta);

                                //Guarda cambios en base de datos
                                await _context.SaveChangesAsync();

                                //Agrega todos los productos ingresados en la tabla temporal de ventas, a la venta
                                foreach (var item in datosTemporal)
                                {
                                    var ProductoDetalle = productos.Where(p => p.Id_Producto.Equals(item.TProducto.Id_Producto)).ToList().Last();
                                    ProductoDetalle.Cantidad_Producto = ProductoDetalle.Cantidad_Producto - item.Cantidad_Venta;

                                    //Declara el objeto de detalles de la venta 
                                    var DetallesVenta = new TDetallesVentas
                                    {
                                        Cantidad_Producto = item.Cantidad_Venta,
                                        Monto_Bruto_Detalle = item.TotalBruto,
                                        Descuento_Detalle = item.TotalDescuentos,
                                        Monto_Neto_Detalle = item.TotalNeto,
                                        Monto_Impuesto_Detalle = item.TotalImpuestos,
                                        TRegistroVentas = RegistroVenta,
                                        TProducto = ProductoDetalle
                                    };

                                    // Agrege el detalle de venta a la base de datos
                                    await _context.AddAsync(DetallesVenta);
                                    // Guarda cambios
                                    await _context.SaveChangesAsync();

                                    //Elimina información de la tabla temporal 
                                    _context.TTemporalVentas.Where(t => t.idTempVentas.Equals(item.idTempVentas)).ToList().ForEach(d => _context.TTemporalVentas.Remove(d));
                                    await _context.SaveChangesAsync();

                                    if (_producto.compruebaCantidadMinimaProducto(ProductoDetalle.Id_Producto))
                                    {
                                        _correo.SendEmailInventario(ProductoDetalle.Nombre_Producto, ProductoDetalle.Cantidad_Producto, ProductoDetalle.Codigo_Producto);
                                    }
                                }
                                valor = true;
                                //Genera ticket
                                GeneraTicket(numeroTicket, nameCliente, nameUser, datosTemporal, montoBruto, montoImpuesto, montoDescuento, monto);
                                //Realiza commit de la transaccion para guardar registros en base de datos
                                transaction.Commit();
                                ventaIniciada = false;
                            }

                            //Validacion de que ingrese opciones de credito o contado
                            if (_dataInput.Credito == false && _dataInput.Contado == false)
                            {
                                _dataInput.ErrorMessage = "Debe seleccionar una forma de Pago";
                                valor = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            //En caso de que se genere una excepcion la carga en la propiedad error message del modelo de datos.
                            _dataInput.ErrorMessage = ex.Message;
                            valor = false;

                            //Realiza rollback de la transaccion
                            transaction.Rollback();
                        }
                    }
                });
            }
            else
            {
                _dataInput.ErrorMessage = "No hay productos registrados";
            }
            return valor;
        }

        public void GeneraTicket(int numeroTicket, string nombreCliente, string userName, List<TTemporalVentas> detalles, float montoBruto ,float montoIVA, float montoDesc, float montoNeto)
        {
            //Genera el ticket que se le entrega al cliente al momento de la venta. 
            LTicket TicketPago = new LTicket();
            //TicketPago.AbreCajon();
            TicketPago.TextoCentro("Libreria y Bazar Dieka");
            TicketPago.TextoIzquierda("Dirección");
            TicketPago.TextoIzquierda("La Trinidad de Moravia");
            TicketPago.TextoIzquierda("Telefono: 22299553");
            TicketPago.LineasGuion();
            TicketPago.TextoCentro("Tiquete de Venta");
            TicketPago.LineasGuion();
            TicketPago.TextoIzquierda($"Tiquete#: {numeroTicket}");
            TicketPago.TextoIzquierda($"Cliente: {nombreCliente}");
            TicketPago.TextoIzquierda($"Fecha: {DateTime.Now}");
            TicketPago.TextoIzquierda($"Usuario: {userName}");
            TicketPago.LineaAsteriscos();
            TicketPago.TextoCentro("Productos Vendidos");
            TicketPago.TextoExtremo("NombreProducto", "Cantidad/Monto");
            foreach (var item in detalles)
            {
                TicketPago.TextoExtremo($"{item.TProducto.Nombre_Producto}", $"Cant {item.Cantidad_Venta} ¢{String.Format("{0:#,###,###,##0.00####}", item.TotalNeto)}");
            }
            TicketPago.LineaAsteriscos();
            TicketPago.TextoDerecha($"Bruto: ¢{String.Format("{0:#,###,###,##0.00####}", montoBruto)}");
            TicketPago.TextoDerecha($"I.V.A: ¢{String.Format("{0:#,###,###,##0.00####}", montoIVA)}");
            TicketPago.TextoDerecha($"Desc: ¢{String.Format("{0:#,###,###,##0.00####}", montoDesc)}");
            TicketPago.TextoDerecha($"Neto: ¢{String.Format("{0:#,###,###,##0.00####}", montoNeto)}");
            TicketPago.LineaAsteriscos();
            TicketPago.TextoCentro("Muchas gracias");
            //TicketPago.CortaTicket();
            TicketPago.ImprimirTicket("Microsoft XPS Document Writer");
        }
    }
}
