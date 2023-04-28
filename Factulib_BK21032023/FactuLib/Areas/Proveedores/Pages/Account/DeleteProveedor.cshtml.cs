using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Proveedores.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    [Authorize]
    [Area("Clientes")]

    public class DeleteProveedorModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static InputModel _dataInput;
        private UploadImage _uploadimage;
        private static InputModelProveedor _dataProveedor1, _dataProveedor2;
        private IWebHostEnvironment _environment;
        private LProveedor _proveedor;


        public DeleteProveedorModel(
           UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _uploadimage = new UploadImage();
            _proveedor = new LProveedor(context);
        }

        public void OnGet(int id)
        {
            if (id.Equals(0))
            {
                _dataProveedor2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataProveedor1 != null || _dataProveedor2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Image = _dataProveedor2.Image;
                }
                else
                {
                    if (_dataProveedor1 != null || _dataProveedor2 != null)
                    {
                        if (_dataProveedor2 != null)
                        {
                            _dataProveedor1 = _dataProveedor2;
                        }

                        Input = new InputModel
                        {
                            CedulaJuridica = _dataProveedor1.CedulaJuridica,
                            NombreProveedor = _dataProveedor1.NombreProveedor,
                            Email = _proveedor.getCorreoProveedor(_dataProveedor1.CedulaJuridica),
                            Image = _dataProveedor1.Image,
                            Telefono = _proveedor.getTelefonoProveedor(_dataProveedor1.CedulaJuridica),
                            Direccion = _context.TDireccionesProveedor.Where(d => d.proveedor.Ced_Jur.Equals(_dataProveedor1.CedulaJuridica)).ToList().Last().Direccion,
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
                Input = new InputModel
                {

                };
            }
            if (_dataInput != null)
            {
                Input = _dataInput;
            }
            if (_dataProveedor2 == null)
            {
                _dataProveedor2 = _dataProveedor1;
            }

            _dataProveedor1 = null;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelProveedor
        {
            public List<SelectListItem> itemProvincia { set; get; }
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
                        var url = $"/Proveedores/Proveedores/Proveedores";
                        _dataProveedor2 = null;
                        _dataInput = null;
                        return Redirect(url);
                    }
                    else
                    {
                        return Redirect("/Proveedores/Delete?id=1");
                    }
                }
                else
                {
                    return Redirect("/Home/ErrorNoAutorizado/");
                }
            }
            else
            {
                _dataProveedor2 = JsonConvert.DeserializeObject<InputModelProveedor>(dataUser);
                return Redirect("/Proveedores/Delete?id=" + id);
            }
        }

        private async Task<bool> DeleteAsync()
        {
            _dataInput = Input;

            var valor = false;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var proveedorData = _proveedor.getProveedor(Input.CedulaJuridica);
                        long Cedula = proveedorData.Last().Ced_Jur;
                        _context.TCreditoProveedor.Where(r => r.TProveedor.Ced_Jur == Cedula).ToList().ForEach(p => _context.TCreditoProveedor.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TDireccionesProveedor.Where(d => d.proveedor.Ced_Jur == Cedula).ToList().ForEach(p => _context.TDireccionesProveedor.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TCorreosProveedor.Where(e => e.proveedor.Ced_Jur == Cedula).ToList().ForEach(p => _context.TCorreosProveedor.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TTelefonosProveedor.Where(t => t.proveedor.Ced_Jur == Cedula).ToList().ForEach(p => _context.TTelefonosProveedor.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.Remove(_context.TProveedor.Single(u => u.Ced_Jur == proveedorData.Last().Ced_Jur));
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        valor = true;
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message + ex.InnerException;
                        transaction.Rollback();
                        valor = false;
                    }
                }
            });
            return valor;
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
                        var proveedorData = _proveedor.getProveedor(_dataInput.CedulaJuridica);
                        if (!proveedorData.Count.Equals(0))
                        {
                            var proveedor = new TProveedor
                            {
                                Nombre_Proveedor = proveedorData.Last().Nombre_Proveedor,
                                Ced_Jur = proveedorData.Last().Ced_Jur,
                                Fecha = proveedorData.Last().Fecha,
                                Imagen = proveedorData.Last().Imagen,
                                IdProveedor = proveedorData.Last().IdProveedor,
                                Enabled = false
                            };
                            _context.Update(proveedor);
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
