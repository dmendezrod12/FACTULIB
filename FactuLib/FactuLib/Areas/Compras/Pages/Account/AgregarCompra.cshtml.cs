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
        private static InputModelCompras _temporal_compras;
        private static byte[] imageByte = null;
        private static bool compraIniciada = false;
        public int _idPagina;
        public string Moneda = "¢";
        public float monto = 0;
        
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
            Moneda = "¢";
        }


        public void OnGet(int id, int IdTemporal, string nombreProveedor,int idProducto, string search, bool error)
        {
            _idPagina = id;
            monto = _compras.getMontoTotal();

           

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
                    Image = _temporal_compras.Image,
                    Date = _temporal_compras.Date,
                };

                if (error == true)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }

                if (Input.idTempCompras != 0)
                {
                    var producto = _context.TProducto.Where(p => p.Nombre_Producto.Replace(" ","").Equals(Input.Nombre.Replace(" ",""))).ToList().Last();
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
                }

                if (nombreProveedor != null)
                {
                    _nombreProveedor = nombreProveedor;
                    Input.Proveedor = _nombreProveedor;
                }

                if (idProducto > 0)
                {
                    var producto = Input.Lista_Productos.List.Where(p => p.Id_Producto.Equals(idProducto)).ToList().Last();
                    Input.Nombre = producto.Nombre_Producto;
                    Input.Descripcion = producto.Descripcion_Producto;
                    Input.Precio = producto.Precio_Costo;
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

        public async Task<IActionResult> OnPost(int delete, String NombreProv,int SelProv, int value, int SelProd, int IdProd)
        {
            if (SelProv == 1)
            {
                //_dataInput2 = Input;
                return Redirect("/Compras/AgregarCompra?area=Compras&nombreProveedor="+NombreProv);
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
                    await ComprasAsync();
                }
                else
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
            }
            return Redirect("/Compras/AgregarCompra?area=Compras");
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel : InputModelCompras
        {
            public string User { get; set; }

            

            public string NombreProveedor { get; set; }

            public string CedJurProveedor { get; set; }

            public int IdProducto { get; set; }

            public int InputIdProducto { get; set; }

            public int NombreProducto { get; set; }

            public float Descuento { get; set; }

            public int CantidadProducto { get; set; }
            public string Moneda { get; set; } = "¢";

            public IFormFile AvatarImage { get; set; }

            public string ErrorMessage { get; set; }

            public List<TTemporalCompras> Compras { get; set; }

            public DataPaginador<InputModelCompras> Temporal_compras { get; set; }

            public DataPaginador<TProveedor> Lista_Proveedores { get; set; }

            public DataPaginador <TProducto> Lista_Productos { get; set; }


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

                    }
                    catch (Exception ex)
                    {

                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
        }

        private async Task ComprasAsync()
        {
            _dataInput = Input;
            var idUser = _userManager.GetUserId(User);
            var proveedor = _context.TProveedor.Where(u => u.Nombre_Proveedor.Replace(" ", "").Equals(_dataInput.Proveedor.Replace(" ", ""))).ToList().Last();
            var datosTemporal = _context.TTemporalCompras.Where(u => u.TUser.IdUser.Equals(idUser) && u.Tproveedor.IdProveedor.Equals(proveedor.IdProveedor)).ToList();
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
                            var _cambio = 0.0m;
                            var user = _context.TUsers.Where(u => u.IdUser.Equals(idUser)).ToList();
                            var nameUser = $"{user[0].Name} {user[0].Apellido1} {user[0].Apellido2}";
                            var monto = _compras.getMontoTotal();
                            var dataTempo = datosTemporal.Last();
                            var deuda = monto - Input.Pagos;
                            //var datosCompras = _context.TRegistroCompras.Where (u=> u.TProveedor.IdProveedor.Equals(dataTempo.Tproveedor.IdProveedor)).ToList();


                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                });
            }
            else
            {
                _dataInput.ErrorMessage = "No hay productos registrados";
            }
        }
    }
}
