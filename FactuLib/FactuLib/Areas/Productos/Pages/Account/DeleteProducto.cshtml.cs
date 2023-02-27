using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Productos.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Productos.Pages.Account
{
    [Authorize]
    [Area("Productos")]
    public class DeleteProductoModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static inputModel _dataInput;
        private UploadImage _uploadimage;
        private static InputModelProductos _dataProducto1, _dataProducto2;
        private IWebHostEnvironment _environment;
        private LProducto _producto;
        public void OnGet(int id)
        {
            if (id.Equals(0))
            {
                _dataProducto2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataProducto1 != null || _dataProducto2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Imagen = _dataProducto2.Imagen;
                }
                else
                {
                    if (_dataProducto1 != null || _dataProducto2 != null)
                    {
                        if (_dataProducto2 != null)
                        {
                            _dataProducto1 = _dataProducto2;
                        }

                        Input = new inputModel
                        {
                            Id_Producto = _dataProducto1.Id_Producto,
                            Nombre_Producto = _dataProducto1.Nombre_Producto,
                            Descripcion_Producto = _dataProducto1.Descripcion_Producto,
                            Codigo_Producto = _dataProducto1.Codigo_Producto,
                            Cantidad_Producto = _dataProducto1.Cantidad_Producto,
                            Imagen = _dataProducto1.Imagen,
                            Precio_Venta = _dataProducto1.Precio_Venta,
                            Precio_Costo = _dataProducto1.Precio_Costo,
                            Descuento_Producto = _dataProducto1.Descuento_Producto,
                            Fecha = _dataProducto1.Fecha
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

                };
            }
            if (_dataInput != null)
            {
                Input = _dataInput;
            }
            if (_dataProducto2 == null)
            {
                _dataProducto2 = _dataProducto1;
            }

            _dataProducto1 = null;
        }

        [BindProperty]

        public inputModel Input { get; set; }
        public class inputModel : InputModelProductos
        {
            public IFormFile AvatarImage { get; set; }
        }

        public async Task<IActionResult> OnPost(String dataUser, int id)
        {
            if (dataUser == null)
            {

                if (User.IsInRole("Administrador"))
                {


                    if (await LogicDeleteAsync())
                    {
                        var url = $"/Productos/Productos/Productos";
                        _dataProducto2 = null;
                        _dataInput = null;
                        return Redirect(url);
                    }
                    else
                    {
                        return Redirect("/Productos/DeleteProducto?id=1");
                    }
                }
                else
                {
                    return Redirect("/Home/ErrorNoAutorizado/");
                }
            }
            else
            {
                _dataProducto1 = JsonConvert.DeserializeObject<InputModelProductos>(dataUser);
                return Redirect("/Productos/DeleteProducto?id=" + id);
            }
        }

        private async Task<bool> LogicDeleteAsync()
        {
            _dataInput = Input;
            var valor = false;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var clientData = _producto.getProducto(_dataInput.Codigo_Producto);
                        if (!clientData.Count.Equals(0))
                        {
                            var producto = new TProducto
                            {
                                Id_Producto = clientData.ToList().Last().Id_Producto,
                                Nombre_Producto = clientData.ToList().Last().Nombre_Producto,
                                Descripcion_Producto = clientData.ToList().Last().Descripcion_Producto,
                                Codigo_Producto = clientData.ToList().Last().Codigo_Producto,
                                Cantidad_Producto = clientData.ToList().Last().Cantidad_Producto,
                                Imagen = clientData.ToList().Last().Imagen,
                                Precio_Venta = clientData.ToList().Last().Precio_Venta,
                                Precio_Costo = clientData.ToList().Last().Precio_Costo,
                                Descuento_Producto = clientData.ToList().Last().Descuento_Producto,
                                TTipoProducto = clientData.ToList().Last().TTipoProducto,
                                Fecha = clientData.ToList().Last().Fecha,
                                Enabled = false
                            };
                            _context.Update(producto);
                            _context.SaveChanges();

                            transaction.Commit();
                            valor = true;
                        }
                        else
                        {
                            _dataInput.ErrorMessage = $"No existen registros en el sistema";
                            valor = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                        valor = false;
                    }
                }
            });
            return valor;
        }

    }
}
