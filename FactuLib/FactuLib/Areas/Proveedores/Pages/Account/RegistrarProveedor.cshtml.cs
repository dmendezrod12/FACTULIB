using FactuLib.Areas.Proveedores.Models;
using FactuLib.Areas.Users.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static FactuLib.Areas.Clientes.Pages.Account.RegistrarClienteModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using FactuLib.Areas.Clientes.Models;
using FactuLib.Models;

namespace FactuLib.Areas.Proveedores.Pages.Account
{
    public class RegistrarProveedorModel : PageModel
    {
        private ApplicationDbContext _context;
        private static InputModel _dataInput;
        private UploadImage _uploadImage;
        private static InputModelProveedor _datosProveedor1, _datosProveedor2;
        private IWebHostEnvironment _environment;
        private LProveedor _proveedor;
        private List<SelectListItem> _ProvinciaItems;

        public RegistrarProveedorModel(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadImage = new UploadImage();
            _proveedor = new LProveedor(context);
        }
        public void OnGet(int id)
        {
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
                Input = new InputModel
                {
                    itemProvincia = _ProvinciaItems,
                };
                if (_dataInput != null)
                {
                    Input.ErrorMessage = _dataInput.ErrorMessage;
                }
                if (id.Equals(0))
                {
                    _datosProveedor2 = null;
                    _datosProveedor1 = null;
                    _dataInput = null;
                }

                if (_dataInput != null || _datosProveedor1 != null || _datosProveedor2 != null)   
                {
                    if (_dataInput != null)
                    {
                        Input = _dataInput;
                        Input.itemProvincia = _ProvinciaItems;
                        Input.AvatarImage = null;
                        if (_datosProveedor2 != null)
                        {
                            Input.Image = _datosProveedor2.Image;
                        }
                    }
                    else
                    {
                        if (_datosProveedor1 != null || _datosProveedor2 != null)
                        {
                            if (_datosProveedor2 != null)
                            {
                                _datosProveedor1 = _datosProveedor2;
                            }
                            Input = new InputModel
                            {
                                CedulaJuridica = _datosProveedor1.CedulaJuridica,
                                NombreProveedor = _datosProveedor1.NombreProveedor,
                                Email = _proveedor.getCorreoProveedor(_datosProveedor1.CedulaJuridica),
                                Image = _datosProveedor1.Image,
                                Telefono = _proveedor.getTelefonoProveedor(_datosProveedor1.CedulaJuridica),
                                Direccion = _context.TDireccionesProveedor.Where(d => d.proveedor.Ced_Jur.Equals(_datosProveedor1.CedulaJuridica)).ToList().Last().Direccion,
                                itemProvincia = _ProvinciaItems,
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
                    Input = new InputModel { itemProvincia = _ProvinciaItems };
                }
                if (_datosProveedor2 == null)
                {
                    _datosProveedor2 = _datosProveedor1;
                }
            }
        }

        public async Task<IActionResult> OnPost (string datosProveedor)
        {
            if(datosProveedor == null)
            {
                if(_datosProveedor2 == null)
                {
                    if (User.IsInRole("Administrador"))
                    {
                        if (_proveedor.NoExisteCedJur(Input.CedulaJuridica))
                        {
                            if (await SaveAsync())
                            {
                                _datosProveedor2 = null;
                                _datosProveedor1 = null;
                                _dataInput = null;
                                return Redirect("/Proveedores/Proveedores?area=Proveedores");
                            }
                            else
                            {
                                return Redirect("/Proveedores/Registrar");
                            }
                        }
                        else
                        {
                            _dataInput = new InputModel();
                            _dataInput = Input;
                            _dataInput.ErrorMessage = "Ya existe numero de cédula juridica registrado";
                            return Redirect("/Proveedores/Registrar?id=1");
                        }
                    }
                    else
                    {
                        return Redirect("/Home/ErrorNoAutorizado/");
                    }        
                }
                else
                {
                    if (User.IsInRole("Administrador"))
                    {
                        if (_proveedor.NoExisteCedJur(Input.CedulaJuridica))
                        {
                            if (await UpdateAsync())
                            {
                                var url = $"/Proveedores/Account/Details?id={_datosProveedor2.CedulaJuridica}";
                                _datosProveedor2 = null;
                                _datosProveedor1 = null;
                                _dataInput = null;
                                return Redirect(url);
                            }
                            else
                            {
                                return Redirect("/Proveedores/Registrar?id=1");
                            }
                        }
                        else
                        {
                            _dataInput = new InputModel();
                            _dataInput = Input;
                            _dataInput.ErrorMessage = "Ya existe numero de cédula juridica registrado";
                            return Redirect("/Proveedores/Registrar?id=1");
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
                _datosProveedor1 = JsonConvert.DeserializeObject<InputModelProveedor>(datosProveedor);
                return Redirect("/Proveedores/Registrar?id=1");
            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelProveedor
        {
            public List<SelectListItem> itemProvincia { set; get; }
            public IFormFile AvatarImage { get; set; }
        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            var listProvincias = _context.TProvincia.ToList();
            var listCantones = _context.TCanton.ToList();
            var distrito = _context.TDistrito.Where(d => d.idDistrito.Equals(Input.Distrito)).ToList().Last();
            var canton = _context.TCanton.Where(c => c.idCanton.Equals(distrito.canton.idCanton)).ToList().Last();
            var provincia = _context.TProvincia.Where(p => p.idProvincia.Equals(canton.provincia.idProvincia)).ToList().Last();
            if (ModelState.IsValid)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var imageByte = await _uploadImage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/DefaultProveedor.png");
                            var proveedor = new TProveedor
                            {
                                Ced_Jur = Input.CedulaJuridica,
                                Nombre_Proveedor = Input.NombreProveedor,
                                Imagen = imageByte,
                                Fecha = DateTime.Now,
                                Enabled = true
                            };
                            await _context.AddAsync(proveedor);
                            _context.SaveChanges();
                            var credito = new TCreditoProveedor
                            {
                                Deuda = 0.0m,
                                Mensual = 0.0m,
                                Cambio = 0.0m,
                                UltimoPago= 0.0m,
                                DeudaActual = 0.0m,
                                Ticket = "0000000000",
                                TProveedor = proveedor
                            };
                            await _context.AddAsync(credito);
                            _context.SaveChanges();
                            var correosProveedor = new TCorreosProveedor
                            {
                                correo= Input.Email,
                                proveedor = proveedor
                            };
                            await _context.AddAsync(correosProveedor);
                            _context.SaveChanges();
                            var telefonosProveedor = new TTelefonosProveedor
                            {
                                telefono = Input.Telefono,
                                proveedor = proveedor
                            };
                            await _context.AddAsync(telefonosProveedor);
                            _context.SaveChanges();
                            var direccionesProveedor = new TDireccionesProveedor
                            {
                                    Direccion = Input.Direccion,
                                    proveedor = proveedor,
                                    idDistrito = Input.Distrito,
                                    TProvincia = provincia,
                                    TCanton = canton,
                        };
                            await _context.AddAsync(direccionesProveedor);
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
                        }
                    }
                });
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

        private async Task<bool> UpdateAsync()
        {
            _dataInput = Input;
            var valor = false;
            byte[] imageByte = null;

            var provincias = _context.TProvincia.ToList();
            var cantones = _context.TCanton.ToList();
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (Input.AvatarImage == null)
                        {
                            imageByte = _datosProveedor2.Image;
                        }
                        else
                        {
                            imageByte = await _uploadImage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                        }

                        var proveedor = new TProveedor { 
                            IdProveedor = _datosProveedor2.idProveedor,
                            Ced_Jur = _datosProveedor2.CedulaJuridica,
                            Nombre_Proveedor = Input.NombreProveedor,
                            Enabled = true,
                            Imagen = imageByte
                        };

                        _context.Update(proveedor);
                        _context.SaveChanges();

                        var correoProveedor = new TCorreosProveedor
                        {
                            correo = Input.Email,
                            proveedor = proveedor
                        };

                        _context.Update(correoProveedor);
                        _context.SaveChanges();

                        var telefonoProveedor = new TTelefonosProveedor
                        {
                            telefono = Input.Telefono,
                            proveedor = proveedor
                        };

                        _context.Update(telefonoProveedor);
                        _context.SaveChanges();

                        var direccionProveedor = new TDireccionesProveedor
                        {
                            Direccion = Input.Direccion,
                            idDistrito = Input.Distrito,
                            TProvincia = provincias.Where(p => p.idProvincia.Equals(Input.Provincia)).Last(),
                            TCanton = cantones.Where(c=> c.idCanton.Equals(Input.Canton)).Last(),
                            proveedor = proveedor
                        };

                        _context.Update(direccionProveedor);
                        _context.SaveChanges();
                        transaction.Commit();
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
            return valor;
        }
    }
}
