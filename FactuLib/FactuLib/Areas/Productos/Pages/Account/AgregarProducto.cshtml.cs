using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static FactuLib.Areas.Clientes.Pages.Account.RegistrarClienteModel;
using System.Collections.Generic;
using FactuLib.Areas.Productos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace FactuLib.Areas.Productos.Pages.Account
{
    [Authorize]
    [Area("Productos")]
    public class AgregarProductoModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static inputModel _dataInput;
        private UploadImage _uploadimage;
        private static InputModelProductos _dataProductos1, _dataProductos2;
        private IWebHostEnvironment _environment;
        private LProducto _producto;
        private List<SelectListItem> _TipoProductoItems;

        public AgregarProductoModel (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _uploadimage = new UploadImage();
            _producto = new LProducto(context);
        }

        public void OnGet(int id, bool error)
        {

            using (_context = new ApplicationDbContext())
            {
                var tiposProductos = _context.TTipoProducto.ToList();
                _TipoProductoItems = new List<SelectListItem>();
                foreach (var item in tiposProductos)
                {
                    _TipoProductoItems.Add(new SelectListItem
                    {
                        Text = item.NombreTipo,
                        Value = item.Id_TipoProducto.ToString()
                    });
                }
            }

            if (id.Equals(0) && error == false)
            {

                _dataProductos2 = null;
                _dataInput = null;
            }
            //if (id.Equals(0) && error == true)
            //{

            //    _dataInput.ErrorMessage = ;

            //}
            if (_dataInput != null || _dataProductos1 != null || _dataProductos2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Imagen = _dataProductos2.Imagen;
                }
                else
                {
                    if (_dataProductos1 != null || _dataProductos2 != null)
                    {
                        if (_dataProductos1 == null && _dataProductos2 != null)
                        {
                            _dataProductos1 = _dataProductos2;
                        }

                        Input = new inputModel
                        {
                            Id_Producto = _dataProductos1.Id_Producto,
                            Codigo_Producto = _dataProductos1.Codigo_Producto,
                            Nombre_Producto = _dataProductos1.Nombre_Producto,
                            Descripcion_Producto = _dataProductos1.Descripcion_Producto,
                            Cantidad_Producto = _dataProductos1.Cantidad_Producto,
                            Precio_Costo = _dataProductos1.Precio_Costo,
                            Precio_Venta = _dataProductos1.Precio_Venta,
                            Descuento_Producto = _dataProductos1.Descuento_Producto,
                            Fecha = _dataProductos1.Fecha,
                            Imagen = _dataProductos1.Imagen,
                            TTipoProducto = _dataProductos1.TTipoProducto,
                        };

                        if (_dataInput != null)
                        {
                            Input.ErrorMessage = _dataInput.ErrorMessage;
                        }
                    }
                }

            }
            else
            {
                Input = new inputModel
                {
                    TipoProductoItems = _TipoProductoItems
                };
            }
            if (_dataInput != null)
            {
                Input = new inputModel {
                    TipoProductoItems = _TipoProductoItems
                };
                Input = _dataInput;
                Input.TipoProductoItems = _TipoProductoItems;
            }
            if (_dataProductos1 == null)
            {
                _dataProductos2 = _dataProductos1;
            }

            _dataProductos1 = null;
        }

        public async Task<IActionResult> OnPost(string DataUser, int id)
        {
            if (DataUser == null)
            {
                if (_dataProductos2 == null)
                {
                    if (User.IsInRole("Administrador"))
                    {
                        if (!_producto.ExisteProducto(Input.Codigo_Producto))
                        {
                            if (await SaveAsync())
                            {

                                _dataProductos2 = null;
                                _dataProductos1 = null;
                                _dataInput = null;
                                return Redirect("/Productos/Productos?area=Productos");
                            }
                            else
                            {

                                _dataProductos2 = _dataInput;
                                return Redirect("/Productos/AgregarProducto?id=1");
                            }
                        }
                        else
                        {
                            _dataInput = new inputModel();
                            _dataInput = Input;
                            _dataInput.ErrorMessage = "Ya existe este código de producto registrado";
                            _dataProductos2 = _dataInput;
                            return Redirect("/Productos/AgregarProducto?error=true");
                        }
                    }
                    else
                    {
                        return Redirect("/Home/ErrorNoAutorizado/");
                    }

                }
                else
                {
                    return Redirect("/Productos/Productos?area=Productos");
                    //if (User.IsInRole("Administrador"))
                    //{
                    //    if (await UpdateAsync())
                    //    {
                    //        var url = $"/Productos/Account/Details?id={_dataProductos2.Id_Producto}";
                    //        _dataProductos2 = null;
                    //        _dataProductos1 = null;
                    //        _dataInput = null;
                    //        return Redirect(url);
                    //    }
                    //    else
                    //    {
                    //        return Redirect("/Producto/AgregarProducto?id=1");
                    //    }
                    //}
                    //else
                    //{
                    //    return Redirect("/Home/ErrorNoAutorizado/");
                    //}
                }

            }
            else
            {
                _dataProductos1 = JsonConvert.DeserializeObject<InputModelProductos>(DataUser);
                return Redirect("/Productos/AgregarProducto?id=1");
            }
        }


        [BindProperty]

        public inputModel Input { get; set; }
        public class inputModel : InputModelProductos
        {
            public IFormFile AvatarImage { get; set; }

            public int IdTipoProducto { get; set; }

            public List<SelectListItem> TipoProductoItems;

        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            string[] PrecioCostoArray = _dataInput.Precio_Costo.Split(",");
            string precioCostoAjustado = "";
            for (int i = 0; i < PrecioCostoArray.Count(); i++)
            {
                PrecioCostoArray[i] = PrecioCostoArray[i].Replace("¢", "");
                PrecioCostoArray[i] = PrecioCostoArray[i].Replace(".", ",");
                precioCostoAjustado += PrecioCostoArray[i];
            }
            decimal PrecioCosto = Decimal.Parse(precioCostoAjustado);
            string[] PrecioVentaArray = _dataInput.Precio_Venta.Split(",");
            string precioVentaAjustado = "";
            for (int i = 0; i < PrecioVentaArray.Count(); i++)
            {
                PrecioVentaArray[i] = PrecioVentaArray[i].Replace("¢", "");
                PrecioVentaArray[i] = PrecioVentaArray[i].Replace(".", ",");
                precioVentaAjustado += PrecioVentaArray[i];
            }
            decimal PrecioVenta = Decimal.Parse(precioVentaAjustado);
            var valor = false;
            if (ModelState.IsValid)
            {
                var productList = _context.TProducto.Where(u => u.Id_Producto.Equals(_dataInput.Codigo_Producto)).ToList();
                if (productList.Count.Equals(0))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                valor = true;
                                var imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/defaultCliente.png");
                                var tipoProducto = _context.TTipoProducto.Where(t => t.Id_TipoProducto.Equals(_dataInput.IdTipoProducto)).ToList().Last();
                                var producto = new TProducto
                                {
                                    Codigo_Producto = _dataInput.Codigo_Producto,
                                    Nombre_Producto = _dataInput.Nombre_Producto,
                                    Descripcion_Producto = _dataInput.Descripcion_Producto,
                                    Cantidad_Producto = _dataInput.Cantidad_Producto,
                                    Precio_Costo = PrecioCosto,
                                    Precio_Venta = PrecioVenta,
                                    Descuento_Producto = _dataInput.Descuento_Producto,
                                    TTipoProducto = tipoProducto,
                                    Fecha = DateTime.Now,
                                    Enabled = true,
                                    Imagen = imageByte,
                                };

                                await _context.AddAsync(producto);
                                _context.SaveChanges();
                                transaction.Commit();
                                //_dataInput = null;
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
                    _dataInput.ErrorMessage = $"El código de producto {_dataInput.Codigo_Producto} ya esta registrado";
                    valor = false;
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
            }

            return valor;
        }

        //private async Task<bool> UpdateAsync()
        //{
        //    _dataInput = Input;
        //    var valor = false;
        //    byte[] imageByte = null;
        //    var listProvincias = _context.TProvincia.ToList();
        //    var listCantones = _context.TCanton.ToList();
        //    var distrito = _context.TDistrito.Where(d => d.idDistrito.Equals(InputProvincia.distrito)).ToList().Last();
        //    var canton = _context.TCanton.Where(c => c.idCanton.Equals(distrito.canton.idCanton)).ToList().Last();
        //    var provincia = _context.TProvincia.Where(p => p.idProvincia.Equals(canton.provincia.idProvincia)).ToList().Last();
        //    var strategy = _context.Database.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () =>
        //    {
        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var clientData = _cliente.getCliente(_dataCliente2.Cedula);
        //                if (!clientData.Count.Equals(0))
        //                {
        //                    if (clientData[0].Cedula.Equals(_dataCliente2.Cedula))
        //                    {
        //                        if (Input.AvatarImage == null)
        //                        {
        //                            imageByte = _dataCliente2.Image;
        //                        }
        //                        else
        //                        {
        //                            imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
        //                        }
        //                        var client = new TClientes
        //                        {
        //                            IdCliente = clientData.Last().IdCliente,
        //                            Cedula = _dataCliente2.Cedula,
        //                            Nombre = Input.Name,
        //                            Apellido1 = Input.Apellido1,
        //                            Apellido2 = Input.Apellido2,
        //                            Credito = Input.Credit,
        //                            Imagen = imageByte,
        //                            Fecha = clientData.Last().Fecha,
        //                            Enabled = true
        //                        };
        //                        _context.Update(client);
        //                        _context.SaveChanges();

        //                        var correos = new TCorreosClientes
        //                        {
        //                            correo = Input.Email,
        //                            cliente = client
        //                        };

        //                        _context.Update(correos);
        //                        _context.SaveChanges();

        //                        var telefonos = new TTelefonoCliente
        //                        {
        //                            telefono = Input.Phone,
        //                            clientes = client
        //                        };

        //                        _context.Update(telefonos);
        //                        _context.SaveChanges();

        //                        var direccionCliente = new TDireccionCliente
        //                        {
        //                            Direccion = Input.Direction,
        //                            clientes = client,
        //                            Cedula = client.Cedula,
        //                            idDistrito = InputProvincia.distrito,
        //                            TProvincia = provincia,
        //                            TCanton = canton,
        //                        };
        //                        _context.Update(direccionCliente);
        //                        _context.SaveChanges();

        //                        transaction.Commit();
        //                        valor = true;
        //                    }
        //                    else
        //                    {
        //                        _dataInput.ErrorMessage = $"No se puede modificar el campo numero de cedula";
        //                        valor = false;
        //                    }
        //                }
        //                else
        //                {
        //                    _dataInput.ErrorMessage = $"El numero de cedula {Input.Cedula} no esta registrado";
        //                    valor = false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _dataInput.ErrorMessage = ex.Message;
        //                transaction.Rollback();
        //                valor = false;
        //            }
        //        }
        //    });
        //    return valor;
        //}

    }
}
