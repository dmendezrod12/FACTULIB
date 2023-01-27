using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    [Authorize]
    [Area("Clientes")]
    public class RegistrarClienteModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private ApplicationDbContext _contextQuery;
        private static inputModel _dataInput;
        private static inputModelProvincia _dataInputDirecciones;
        private UploadImage _uploadimage;
        private static InputModelRegistrar _dataCliente1, _dataCliente2; 
        private IWebHostEnvironment _environment;
        private LCliente _cliente;
        private List<SelectListItem> _ProvinciaItems;


        public RegistrarClienteModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _contextQuery = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _uploadimage = new UploadImage();
            _cliente = new LCliente(context);
        }
        public void OnGet(int id, bool error)
        {
            //_dataCliente2 = null;
            using (_context = new ApplicationDbContext())
            {
                var provincias = _context.TProvincia.ToList();
                _ProvinciaItems = new List<SelectListItem>();
                foreach (var item in provincias)
                {
                    _ProvinciaItems.Add(new SelectListItem
                    {
                        Text = item.nombreProvincia,
                        Value = item.idProvincia.ToString()
                    });
                }
                InputProvincia = new inputModelProvincia
                {
                    itemProvincia = _ProvinciaItems
                };
            }

            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Cedula Fisica Nacional", Value = "1" });
            lst.Add(new SelectListItem() { Text = "Cedula Juridica Nacional", Value = "2" });
            lst.Add(new SelectListItem() { Text = "Documento de identificación de Migración y extranjeria (DIMEX)", Value = "3" });
            lst.Add(new SelectListItem() { Text = "Numero de Identificacion tributario especial (NITE)", Value = "4" });



            if (id.Equals(0) && error == false)
            {
                
                _dataCliente2 = null;
                _dataInput = null;
            }
            //if (id.Equals(0) && error == true)
            //{

            //    _dataInput.ErrorMessage = ;

            //}
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
                        if (_dataCliente1 == null && _dataCliente2 != null)
                        {
                            _dataCliente1 = _dataCliente2;
                        }

                        Input = new inputModel
                        {
                            Name = _dataCliente1.Name,
                            Apellido1 = _dataCliente1.Apellido1,
                            Apellido2 = _dataCliente1.Apellido2,
                            Cedula = _dataCliente1.Cedula,
                            CedulaActulizar = _dataCliente1.Cedula,
                            CedulaJuridica = _dataCliente1.CedulaJuridica,
                            CedulaNITE = _dataCliente1.CedulaNITE,
                            CedulaResidencia = _dataCliente1.CedulaResidencia,
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
                    opcionCedula = lst
                };
            }
            if (_dataInput != null)
            {
                //Input = _dataInput;
                Input = new inputModel
                {
                    opcionCedula = lst,
                    ErrorMessage = _dataInput.ErrorMessage
                };
                //Input.ErrorMessage = _dataInput.ErrorMessage;
                //Input.opcionCedula = lst;
            }
            if (_dataCliente2 == null)
            {
                _dataCliente2 = _dataCliente1;
            }
            
            _dataCliente1 = null;
            
        }

        [BindProperty]

        public inputModelProvincia InputProvincia { get; set; }
        public class inputModelProvincia
        {
            public List<SelectListItem> itemProvincia { set; get; }

            [Required(ErrorMessage = "Seleccione una Provincia")]

            public int provincia { get; set; }

            [Required(ErrorMessage = "Seleccione un Canton")]

            public int canton { get; set; }

            [Required(ErrorMessage = "Seleccione un Distrito")]

            public int distrito { get; set; }

            
        }

        [BindProperty]

        public inputModel Input { get; set; }
        public class inputModel : InputModelRegistrar 
        {
            public IFormFile AvatarImage { get; set; }

            public List<SelectListItem> opcionCedula { get; set; }

            public string OpcionCedula { get; set; }
        }

        public async Task<IActionResult> OnPost(String DataUser, int id)
        {
            if (Input.CedulaJuridica > 0 && Input.CedulaJuridica != 9999999999)
            {
                Input.Cedula = Input.CedulaJuridica;
            }
            if (Input.CedulaResidencia > 0 && Input.CedulaResidencia != 999999999999)
            {
                Input.Cedula = Input.CedulaResidencia;
            }
            if (Input.CedulaNITE > 0 && Input.CedulaNITE != 9999999999)
            {
                Input.Cedula = Input.CedulaNITE;
            }
            if (DataUser == null)
            {
                if (_dataCliente2 == null)
                {
                    if (User.IsInRole("Administrador"))
                    {
                        if (!_cliente.ExisteCliente(Input.Cedula))
                        {
                            if (await SaveAsync())
                            {

                                _dataCliente2 = null;
                                _dataCliente1 = null;
                                _dataInput = null;
                                return Redirect("/Clientes/Clientes?area=Clientes");
                            }
                            else
                            {
                                
                                _dataCliente2 = _dataInput;
                                return Redirect("/Clientes/Register?id=1");
                            }
                        }
                        else
                        {
                            _dataInput = new inputModel();
                            _dataInput = Input;
                            _dataInput.ErrorMessage = "Ya existe este numero de cedula registrado";
                            _dataCliente2 = _dataInput;
                            return Redirect("/Clientes/Register?error=true");
                        }
                    }
                    else
                    {
                        return Redirect("/Home/ErrorNoAutorizado/");
                    }

                }
                else
                {
                    if (User.IsInRole ("Administrador"))
                    {
                        if (await UpdateAsync())
                        {
                            var url = $"/Clientes/Account/Details?id={_dataCliente2.Cedula}";
                            _dataCliente2 = null;
                            _dataCliente1 = null;
                            _dataInput = null;
                            return Redirect(url);
                        }
                        else
                        {
                            return Redirect("/Clientes/Register?id=1");
                        }
                    }
                    else
                    {
                        return Redirect("/Home/ErrorNoAutorizado/");
                    }
                }

            }
            else
            {
                _dataCliente1 = JsonConvert.DeserializeObject<InputModelRegistrar>(DataUser);
                _dataCliente1.CedulaJuridica = 9999999999;
                _dataCliente1.CedulaNITE = 9999999999;
                _dataCliente1.CedulaResidencia = 999999999999;
                return Redirect("/Clientes/Register?id=1");

            }
        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;

            // Coloca el input de Cedula segun el tipo de Cedula

            if (_dataInput.OpcionCedula == "1") {
                _dataInput.Cedula = Input.Cedula;
            }
            if (_dataInput.OpcionCedula == "2")
            {
                _dataInput.Cedula = Input.CedulaJuridica;
            }
            if (_dataInput.OpcionCedula == "3")
            {
                _dataInput.Cedula = Input.CedulaResidencia;
            }
            if (_dataInput.OpcionCedula == "4")
            {
                _dataInput.Cedula = Input.CedulaNITE;
            }
            _dataInputDirecciones = InputProvincia;
            var valor = false;
            if (ModelState.IsValid)
            {
                var clientList = _context.TClientes.Where(u => u.Cedula.Equals(_dataInput.Cedula)).ToList();
                var listProvincias = _context.TProvincia.ToList();
                var listCantones = _context.TCanton.ToList();
                var distrito = _context.TDistrito.Where(d=> d.idDistrito.Equals(InputProvincia.distrito)).ToList().Last();
                var canton = _context.TCanton.Where(c => c.idCanton.Equals(distrito.canton.idCanton)).ToList().Last();
                var provincia = _context.TProvincia.Where(p => p.idProvincia.Equals(canton.provincia.idProvincia)).ToList().Last();
                if (clientList.Count.Equals(0))
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
                                var client = new TClientes
                                {
                                    Nombre = Input.Name,
                                    Apellido1 = Input.Apellido1,
                                    Apellido2 = Input.Apellido2,
                                    Cedula = _dataInput.Cedula,
                                    Imagen = imageByte,
                                    Credito = Input.Credit,
                                    Fecha = DateTime.Now,
                                    Enabled = true
                                };

                                await _context.AddAsync(client);
                                _context.SaveChanges();

                                var correos = new TCorreosClientes
                                {
                                    correo = Input.Email,
                                    cliente = client
                                };

                                await _context.AddAsync(correos);
                                _context.SaveChanges();

                                var telefonos = new TTelefonoCliente
                                {
                                    telefono = Input.Phone,
                                    clientes = client
                                };

                                await _context.AddAsync(telefonos);
                                _context.SaveChanges();


                                var credito = new TCreditoClientes
                                {
                                    Deuda = 0.0m,
                                    Mensual = 0.0m,
                                    Cambio = 0.0m,
                                    UltimoPago = 0.0m,
                                    DeudaActual = 0.0m,
                                    Ticket = "0000000000",
                                    CedulaCliente = client.Cedula,
                                    TClients = client
                                };
                                await _context.AddAsync(credito);
                                _context.SaveChanges();
                                

                                var DireccionCliente = new TDireccionCliente
                                {
                                    Direccion = Input.Direction,
                                    clientes = client,
                                    idDistrito = InputProvincia.distrito,
                                    TProvincia = provincia,
                                    TCanton = canton,
                                    Cedula = client.Cedula
                                };
                                await _context.AddAsync(DireccionCliente);
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
                    _dataInput.ErrorMessage = $"El numero de cédula {_dataInput.Cedula} ya esta registrado";
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

        private async Task<bool> UpdateAsync()
        {
            _dataInput = Input;
            var valor = false;
            byte[] imageByte = null;
            var listProvincias = _context.TProvincia.ToList();
            var listCantones = _context.TCanton.ToList();
            var distrito = _context.TDistrito.Where(d => d.idDistrito.Equals(InputProvincia.distrito)).ToList().Last();
            var canton = _context.TCanton.Where(c => c.idCanton.Equals(distrito.canton.idCanton)).ToList().Last();
            var provincia = _context.TProvincia.Where(p => p.idProvincia.Equals(canton.provincia.idProvincia)).ToList().Last();
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var clientData = _cliente.getCliente(_dataCliente2.Cedula);
                        if (!clientData.Count.Equals(0))
                        {
                            if (clientData[0].Cedula.Equals(_dataCliente2.Cedula))
                            {
                                if (Input.AvatarImage == null)
                                {
                                    imageByte = _dataCliente2.Image;
                                }
                                else
                                {
                                    imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                                }
                                var client = new TClientes
                                {
                                    IdCliente = clientData.Last().IdCliente,
                                    Cedula = _dataCliente2.Cedula,
                                    Nombre = Input.Name,
                                    Apellido1 = Input.Apellido1,
                                    Apellido2 = Input.Apellido2,
                                    Credito = Input.Credit,
                                    Imagen = imageByte,
                                    Fecha = clientData.Last().Fecha,
                                    Enabled = true
                                };
                                _context.Update(client);
                                _context.SaveChanges();

                                var correos = new TCorreosClientes
                                {
                                    correo = Input.Email,
                                    cliente = client
                                };

                                _context.Update(correos);
                                _context.SaveChanges();

                                var telefonos = new TTelefonoCliente
                                {
                                    telefono = Input.Phone,
                                    clientes = client
                                };

                                _context.Update(telefonos);
                                _context.SaveChanges();

                                var direccionCliente = new TDireccionCliente
                                {
                                    Direccion = Input.Direction,
                                    clientes = client,
                                    Cedula = client.Cedula,
                                    idDistrito = InputProvincia.distrito,
                                    TProvincia = provincia,
                                    TCanton = canton,
                                };
                                _context.Update(direccionCliente);
                                _context.SaveChanges();

                                transaction.Commit();
                                valor = true;
                            }
                            else
                            {
                                _dataInput.ErrorMessage = $"No se puede modificar el campo numero de cedula";
                                valor = false;
                            }      
                        }
                        else
                        {
                            _dataInput.ErrorMessage = $"El numero de cedula {Input.Cedula} no esta registrado";
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
