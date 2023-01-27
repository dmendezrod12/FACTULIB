using FactuLib.Areas.Clientes.Models;
using FactuLib.Areas.Users.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    [Authorize]
    [Area("Clientes")]
    public class DeleteClienteModel : PageModel
    {
        public DeleteClienteModel(
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
            _cliente = new LCliente(context);
        }

        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private static inputModel _dataInput;
        private UploadImage _uploadimage;
        private static InputModelRegistrar _dataCliente1, _dataCliente2;
        private IWebHostEnvironment _environment;
        private LCliente _cliente;
        public void OnGet(int id)
        {
            //_dataCliente2 = null;

            if (id.Equals(0))
            {
                _dataCliente2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataCliente1 != null || _dataCliente2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.AvatarImage = null;
                    Input.Image = _dataCliente2.Image;
                }
                else
                {
                    if (_dataCliente1 != null || _dataCliente2 != null)
                    {
                        if (_dataCliente2 != null)
                        {
                            _dataCliente1 = _dataCliente2;
                        }

                        Input = new inputModel
                        {
                            Name = _dataCliente1.Name,
                            Apellido1 = _dataCliente1.Apellido1,
                            Apellido2 = _dataCliente1.Apellido2,
                            Cedula = _dataCliente1.Cedula,
                            Email = _dataCliente1.Email,
                            Image = _dataCliente1.Image,
                            Phone = _dataCliente1.Phone,
                            Direction = _dataCliente1.Direction,
                            Credit = _dataCliente1.Credit
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
            if (_dataCliente2 == null)
            {
                _dataCliente2 = _dataCliente1;
            }

            _dataCliente1 = null;

        }
        [BindProperty]

        public inputModel Input { get; set; }
        public class inputModel : InputModelRegistrar
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
                        var url = $"/Clientes/Clientes/Clientes";
                        _dataCliente2 = null;
                        _dataInput = null;
                        return Redirect(url);
                    }
                    else
                    {
                        return Redirect("/Clientes/DeleteCliente/?id=1");
                    }
                }
                else
                {
                    return Redirect("/Home/ErrorNoAutorizado/");
                }
            }
            else
            {
                _dataCliente1 = JsonConvert.DeserializeObject<InputModelRegistrar>(dataUser);
                return Redirect("/Clientes/DeleteCliente/?id=" + id);
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
                        var clientData = _cliente.getCliente(Input.Cedula);
                        long Cedula = clientData.Last().Cedula;
                        _context.TCreditoClientes.Where(r => r.TClients.Cedula == Cedula).ToList().ForEach(p => _context.TCreditoClientes.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TDireccionCliente.Where(d => d.clientes.Cedula == Cedula).ToList().ForEach(p => _context.TDireccionCliente.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TCorreosClientes.Where(e => e.cliente.Cedula == Cedula).ToList().ForEach(p => _context.TCorreosClientes.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.TTelefonoCliente.Where(t => t.clientes.Cedula == Cedula).ToList().ForEach(p => _context.TTelefonoCliente.Remove(p));
                        await _context.SaveChangesAsync();
                        _context.Remove(_context.TClientes.Single(u => u.Cedula == clientData.Last().Cedula));
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
                        var clientData = _cliente.getCliente(_dataInput.Cedula);
                        if (!clientData.Count.Equals(0))
                        {
                            var client = new TClientes
                            {
                                IdCliente = clientData.Last().IdCliente,
                                Cedula = clientData.Last().Cedula,
                                Nombre = clientData.Last().Nombre,
                                Apellido1 = clientData.Last().Apellido1,
                                Apellido2 = clientData.Last().Apellido2,
                                Credito = clientData.Last().Credito,
                                Imagen = clientData.Last().Imagen,
                                Enabled = false
                            };
                            _context.Update(client);
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
