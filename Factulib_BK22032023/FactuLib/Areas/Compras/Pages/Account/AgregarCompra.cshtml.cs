using FactuLib.Areas.Compras.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Users.Models;
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
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Compras.Pages.Account
{
    [Authorize]
    public class AgregarCompraModel : PageModel
    {
        private static InputModel _dataInput, _dataInput2;
        private UploadImage _uploadImage;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private UserManager<IdentityUser> _userManager;
        private LCompras _compras;
        private LApertura _apertura;
        private static InputModelCompras _temporal_compras;
        private static byte[] imageByte = null;
        private static bool compraIniciada = false;
        public int _idPagina;
        public string Moneda = "�";
        public float monto, montoDescuento, montoImpuesto, montoBruto = 0;
        public static string _proveedorSeleccionado;
        public static long _CedulaProveedor = 0;

        public static string _nombreProveedor = "";

        public AgregarCompraModel(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            UserManager<IdentityUser> userManager)
        {
            _uploadImage = new UploadImage();
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _compras = new LCompras(context);
            _apertura = new LApertura(context);
            Moneda = "�";
        }


        public void OnGet(int id, int IdTemporal, string nombreProveedor, int idProducto, string search, bool error, long CedulaProveedor)
        {
            _idPagina = id;
            monto = _compras.getMontoTotal();
            montoBruto = _compras.getMontoBruto();
            montoDescuento = _compras.getMontoDescuentos();
            montoImpuesto = _compras.getMontoImpuestos();

            if (_dataInput != null)
            {
                Input = _dataInput;
                Input.AvatarImage = null;
                Input.Image = _dataInput.Image;
                Input.Temporal_compras = _compras.Get_Temporal_Compras(_idPagina, 5, search, Request);
                Input.Lista_Proveedores = _compras.GetProveedoreslista(_idPagina, 5, null, Request);
                Input.Lista_Productos = _compras.GetProductosLista(_idPagina, 5, null, Request);

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
                _temporal_compras = _compras.getCompras(IdTemporal);
                Input = new InputModel
                {
                    Temporal_compras = _compras.Get_Temporal_Compras(_idPagina, 5, search, Request),
                    Lista_Proveedores = _compras.GetProveedoreslista(_idPagina, 5, null, Request),
                    Lista_Productos = _compras.GetProductosLista(_idPagina, 5, null, Request),
                    idTempCompras = _temporal_compras.idTempCompras,
                    Nombre = _temporal_compras.Nombre,
                    Descripcion = _temporal_compras.Descripcion,
                    Cantidad = _temporal_compras.Cantidad,
                    Precio = _temporal_compras.Precio,
                    TotalBruto = _temporal_compras.TotalBruto,
                    Descuento = _temporal_compras.Descuento,
                    Image = _temporal_compras.Image,
                    Date = _temporal_compras.Date,
                };

                if (error == true)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }

                if (Input.idTempCompras != 0)
                {
                    var producto = _context.TProducto.Where(p => p.Nombre_Producto.Replace(" ", "").Equals(Input.Nombre.Replace(" ", ""))).ToList().Last();
                    Input.TProducto = producto;
                    Input.InputIdProducto = producto.Id_Producto;
                }
                if (_temporal_compras.Tproveedor != null)
                {
                    Input.Tproveedor = _temporal_compras.Tproveedor;
                    Input.Proveedor = _temporal_compras.Tproveedor.Nombre_Proveedor;
                }

                if (_nombreProveedor != "")
                {
                    Input.Proveedor = _nombreProveedor;
                    Input.CedProveedorSeleccionado = _CedulaProveedor;
                }

                if (nombreProveedor != null)
                {
                    _nombreProveedor = nombreProveedor;
                    _CedulaProveedor = CedulaProveedor;
                    Input.Proveedor = _nombreProveedor;
                    Input.CedProveedorSeleccionado = _CedulaProveedor;
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

                if (_temporal_compras.TUser != null)
                {
                    Input.TUser = _temporal_compras.TUser;
                    Input.User = _temporal_compras.TUser.Name;
                }
            }
        }

        public async Task<IActionResult> OnPost(int delete, String NombreProv, int SelProv, int value, int SelProd, int IdProd)
        {
            if (SelProv == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Compras/AgregarCompra?area=Compras&nombreProveedor=" + NombreProv);
            }

            if (SelProd == 1)
            {
                return Redirect("/Compras/AgregarCompra?area=Compras&idProducto=" + IdProd);
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
                        if (await ComprasAsync())
                        {
                            return Redirect("/Compras/AgregarCompra?area=Compras");
                        }
                        else
                        {
                            return Redirect("/Compras/AgregarCompra?area=Compras&error=true");
                        }
                    }
                    else
                    {
                        return Redirect("/Cajas/Apertura_Caja?area=Cajas");
                    }
                       
                }
                else
                {
                    if (!_apertura.ValidaApertura(_userManager.GetUserId(User)))
                    {
                        if (_temporal_compras.idTempCompras == 0)
                        {
                            if (await SaveAsync())
                            {
                                return Redirect("/Compras/AgregarCompra?area=Compras");
                            }
                            else
                            {
                                return Redirect("/Compras/AgregarCompra?area=Compras&error=true");
                            }
                        }
                        else
                        {
                            await EditAsync();
                        }
                    }
                    else
                    {
                        return Redirect("/Cajas/Apertura_Caja?area=Cajas");
                    }    
                    
                }
            }
            return Redirect("/Compras/AgregarCompra?area=Compras");
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelCompras
        {
            public string User { get; set; }

            public long CedProveedorSeleccionado { get; set; }

            public string NombreProveedor { get; set; }

            public string CedJurProveedor { get; set; }

            public int IdProducto { get; set; }

            public int InputIdProducto { get; set; }

            public int NombreProducto { get; set; }

            

            public int CantidadProducto { get; set; }
            public string Moneda { get; set; } = "�";

            public IFormFile AvatarImage { get; set; }

            public string ErrorMessage { get; set; }

            public List<TTemporalCompras> Compras { get; set; }

            public DataPaginador<InputModelCompras> Temporal_compras { get; set; }

            public DataPaginador<TProveedor> Lista_Proveedores { get; set; }

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
                var proveedor = _context.TProveedor.Where(p => p.Nombre_Proveedor.Replace(" ", "").Equals(_dataInput.Proveedor.Replace(" ", ""))).ToList();
                if (0 < proveedor.Count)
                {
                    var dataProveedor = proveedor.Last();
                    var idUser = _userManager.GetUserId(User);
                    bool proveedorInvalido = false;
                    var dataCompras = _context.TTemporalCompras.Where(t => t.Tproveedor.IdProveedor.Equals(dataProveedor.IdProveedor) && t.TUser.IdUser.Equals(idUser)).ToList();

                    //if (dataCompras.Count == 0 && compraIniciada == false)
                    //{
                    //    compraIniciada = true; 
                    //}


                    if (dataCompras.Count == 0 && compraIniciada == true)
                    {
                        proveedorInvalido = true;
                    }

                    if (dataCompras.Count == 0 && proveedorInvalido == false)
                    {
                        await SaveAsync();
                        compraIniciada = true;
                    }
                    else
                    {
                        if (proveedorInvalido == false)
                        {
                            await SaveAsync();
                        }
                        else
                        {
                            _dataInput.ErrorMessage = "Finalizar la compra del proveedor en lista";
                        }
                    }
                    async Task SaveAsync()
                    {
                        _proveedorSeleccionado = _dataInput.Proveedor;
                        var strategy = _context.Database.CreateExecutionStrategy();
                        var userData = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList().Last();
                        var producto = _context.TProducto.Where(p => p.Id_Producto.Equals(_dataInput.InputIdProducto)).ToList().Last();
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
                                    var compras = new TTemporalCompras
                                    {
                                        TotalBruto = monto,
                                        TotalDescuentos = descuento,
                                        TotalImpuestos = impuesto,
                                        TotalNeto = montoNeto,
                                        Cantidad_Compra = _dataInput.Cantidad,
                                        Tproveedor = dataProveedor,
                                        TProducto = producto,
                                        TUser = userData,
                                        Date = DateTime.Now
                                    };

                                    await _context.AddAsync(compras);
                                    _context.SaveChanges();
                                    transaction.Commit();
                                    _dataInput = null;
                                    valor = true;
                                }
                                catch (Exception ex)
                                {

                                    _dataInput.ErrorMessage = ex.Message;
                                    transaction.Rollback();
                                    valor = false;

                                    if (dataCompras.Count.Equals(0) && compraIniciada == true)
                                    {
                                        compraIniciada = false;
                                    }
                                }
                            }
                        });
                    }
                }
                else
                {
                    _dataInput.ErrorMessage = "El proveedor no esta registrado";
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
            List<TTemporalCompras> tempCompras = null;
            if (Input.AvatarImage != null)
            {
                imageByte = await _uploadImage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/agregarCompra.png");
            }
            var proveedor = _context.TProveedor.Where(u => u.Nombre_Proveedor.Replace(" ", "").Equals(_dataInput.Proveedor.Replace(" ", ""))).ToList();

            if (0 < proveedor.Count)
            {
                var datosProveedor = proveedor.Last();
                var idUser = _userManager.GetUserId(User);
                using (var dbContext = new ApplicationDbContext())
                {
                    var proveedores = dbContext.TProveedor.ToList();
                    var usuarios = dbContext.TUsers.ToList();
                    tempCompras = dbContext.TTemporalCompras.Where(p => p.Tproveedor.IdProveedor.Equals(datosProveedor.IdProveedor) && p.TUser.IdUser.Equals(idUser)).ToList();
                }
                if (tempCompras[0].Tproveedor.IdProveedor.Equals(datosProveedor.IdProveedor))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    var usuario = _context.TUsers.Where(u => u.IdUser.Equals(tempCompras[0].TUser.IdUser)).ToList().Last();
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
                                var compras = new TTemporalCompras
                                {
                                    idTempCompras = _temporal_compras.idTempCompras,
                                    Date = _temporal_compras.Date,
                                    TotalDescuentos = descuento,
                                    TotalImpuestos = impuesto,
                                    TotalNeto = montoNeto,
                                    TotalBruto = monto,
                                    Cantidad_Compra = _dataInput.Cantidad,
                                    Tproveedor = datosProveedor,
                                    TProducto = producto,
                                    TUser = usuario,
                                };
                                _context.Update(compras);
                                _context.SaveChanges();
                                transaction.Commit();
                                _dataInput = null;
                                _temporal_compras = null;
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
                    _dataInput.ErrorMessage = "Finalizar la compra de proveedor en lista";
                }
            }
            else
            {
                _dataInput.ErrorMessage = "El proveedor no esta registrado";
            }
            return valor;
        }

        private async Task DeleteAsync(int id)
        {
            TTemporalCompras datosCompras = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            datosCompras = dbContext.TTemporalCompras.Where(t => t.idTempCompras.Equals(id)).ToList().Last();
                        }
                        _context.Remove(datosCompras);
                        _context.SaveChanges();
                        transaction.Commit();
                        _dataInput = null;
                        _temporal_compras = null;
                        var listaTotalCompras = _context.TTemporalCompras.ToList();
                        if (listaTotalCompras.Count.Equals(0))
                        {
                            compraIniciada = false;
                        }

                    }
                    catch (Exception ex)
                    {

                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
        }

        private async Task<bool> ComprasAsync()
        {
            _dataInput = Input;
            var valor = false;
            var idUser = _userManager.GetUserId(User);
            var proveedor = _context.TProveedor.Where(u => u.Nombre_Proveedor.Replace(" ", "").Equals(_proveedorSeleccionado.Replace(" ", ""))).ToList().Last();
            var datosTemporal = _context.TTemporalCompras.Where(u => u.TUser.IdUser.Equals(idUser) && u.Tproveedor.IdProveedor.Equals(proveedor.IdProveedor)).ToList();
            var productos = _context.TProducto.ToList();
            var proveedores = _context.TProveedor.ToList();
            var creditosProveedor = _context.TCreditoProveedor.ToList();
            if (datosTemporal.Count > 0)
            {
                var strategy  = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () => {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            string _ticket = null;
                            var dateNow = DateTime.Now;
                            var metodoPago = 0;
                            var _cambio = 0.0m;
                            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList().Last();
                            var nameUser = $"{user.Name} {user.Apellido1} {user.Apellido2}";
                            var monto = _compras.getMontoTotal();
                            var montoBruto = _compras.getMontoBruto();
                            var montoDescuento = _compras.getMontoDescuentos();
                            var montoImpuesto = _compras.getMontoImpuestos();
                            var dataTempo = datosTemporal.Last();
                            var deuda = monto - Input.Pagos;
                            //var datosCompras = _context.TRegistroCompras.Where (u=> u.TProveedor.IdProveedor.Equals(dataTempo.Tproveedor.IdProveedor)).ToList();

                            if (_dataInput.Contado == true)
                            {
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

                                _cambio = (decimal)(_dataInput.Pagos - monto);

                                var RegistroCompra = new TRegistroCompras
                                {
                                    NumeroFacturaProveedor = _dataInput.FacturaProveedor,
                                    Total_Bruto = montoBruto,
                                    Total_Descuento = montoDescuento,
                                    Total_IVA = montoImpuesto,
                                    Total_Neto = monto,
                                    MetodoPago = metodoPago,
                                    DineroRecibido = _dataInput.Pagos,
                                    CambioCompra = _cambio,
                                    Fecha_Compra = DateTime.Now,
                                    TProveedor = proveedor,
                                    TUser = user,
                                    Estado_Compra = true
                                };

                                await _context.AddAsync(RegistroCompra);
                                await _context.SaveChangesAsync();

                                foreach (var item in datosTemporal)
                                {
                                   var ProductoDetalle = productos.Where(p => p.Id_Producto.Equals(item.TProducto.Id_Producto)).ToList().Last();
                                   ProductoDetalle.Cantidad_Producto = ProductoDetalle.Cantidad_Producto + item.Cantidad_Compra;

                                    var DetalleCompra = new TDetallesCompras
                                    {
                                        Cantidad_Producto = item.Cantidad_Compra,
                                        Monto_Bruto_Detalle = item.TotalBruto,
                                        Descuento_Detalle = item.TotalDescuentos,
                                        Monto_Neto_Detalle = item.TotalNeto,
                                        Monto_Impuesto_Detalle = item.TotalImpuestos,
                                        TRegistroCompras = RegistroCompra,
                                        TProducto = ProductoDetalle
                                    };

                                    await _context.AddAsync(DetalleCompra);
                                    await _context.SaveChangesAsync();

                                    _context.TTemporalCompras.Where(t => t.idTempCompras.Equals(item.idTempCompras)).ToList().ForEach(d => _context.TTemporalCompras.Remove(d));
                                    await _context.SaveChangesAsync();
                                }
                                valor = true; 
                                transaction.Commit();
                                compraIniciada = false;
                            }
                            if (_dataInput.Credito == true)
                            {
                                var credito = creditosProveedor.Where(c => c.TProveedor.IdProveedor.Equals(proveedor.IdProveedor)).ToList().Last();

                                var deudaActualAnterior = (float)credito.DeudaActual;
                                var deudaNueva = deudaActualAnterior + monto;
                                var deudaActualNueva = deudaNueva;

                                var CreditoProveedor = new TCreditoProveedor
                                {
                                    //idCredito = credito.idCredito,
                                    Deuda = (Decimal)deudaNueva,
                                    Mensual = credito.Mensual,
                                    Cambio = credito.Cambio,
                                    UltimoPago = credito.UltimoPago,
                                    FechaPago = credito.FechaPago,
                                    DeudaActual = (Decimal)deudaActualNueva,
                                    FechaDeuda = credito.FechaDeuda,
                                    Ticket = credito.Ticket,
                                    FechaLimite = credito.FechaLimite,
                                    TProveedor = proveedor,

                                };

                                _context.Update(CreditoProveedor);
                                _context.SaveChanges();

                                var RegistroCompra = new TRegistroCompras
                                {
                                    NumeroFacturaProveedor = _dataInput.FacturaProveedor,
                                    Total_Bruto = montoBruto,
                                    Total_Descuento = montoDescuento,
                                    Total_IVA = montoImpuesto,
                                    Total_Neto = monto,
                                    MetodoPago = 0,
                                    DineroRecibido = 0,
                                    CambioCompra = 0,
                                    Fecha_Compra = DateTime.Now,
                                    TProveedor = proveedor,
                                    TUser = user,
                                    Estado_Compra = true
                                };

                                await _context.AddAsync(RegistroCompra);
                                await _context.SaveChangesAsync();

                                foreach (var item in datosTemporal)
                                {
                                    var ProductoDetalle = productos.Where(p => p.Id_Producto.Equals(item.TProducto.Id_Producto)).ToList().Last();
                                    ProductoDetalle.Cantidad_Producto = ProductoDetalle.Cantidad_Producto + item.Cantidad_Compra;

                                    var DetalleCompra = new TDetallesCompras
                                    {
                                        Cantidad_Producto = item.Cantidad_Compra,
                                        Monto_Bruto_Detalle = item.TotalBruto,
                                        Descuento_Detalle = item.TotalDescuentos,
                                        Monto_Neto_Detalle = item.TotalNeto,
                                        Monto_Impuesto_Detalle = item.TotalImpuestos,
                                        TRegistroCompras = RegistroCompra,
                                        TProducto = ProductoDetalle
                                    };

                                    await _context.AddAsync(DetalleCompra);
                                    await _context.SaveChangesAsync();

                                    _context.TTemporalCompras.Where(t => t.idTempCompras.Equals(item.idTempCompras)).ToList().ForEach(d => _context.TTemporalCompras.Remove(d));
                                    await _context.SaveChangesAsync();
                                }
                                valor = true;
                                transaction.Commit();
                                compraIniciada = false;
                            }

                            if (_dataInput.Credito == false && _dataInput.Contado == false)
                            {
                                _dataInput.ErrorMessage = "Debe seleccionar una forma de Pago";
                                valor = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            _dataInput.ErrorMessage = ex.Message;
                            valor = false;
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
    }
}