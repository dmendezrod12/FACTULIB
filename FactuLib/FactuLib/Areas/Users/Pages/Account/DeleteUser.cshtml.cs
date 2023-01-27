using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactuLib.Areas.Users.Models;
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
using static FactuLib.Areas.Clientes.Pages.Account.RegistrarClienteModel;

namespace FactuLib.Areas.Users.Pages.Account
{
    [Authorize]
    [Area("Users")]
    public class DeleteUser : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private LUsersRoles _usersRole;
        private static inputModel _dataInput;
        private UploadImage _uploadImage;
        private IWebHostEnvironment _environment;
        private static InputModelRegister _dataUserActualizar1, _dataUserActualizar2;

        public DeleteUser(
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
            _usersRole = new LUsersRoles();
            _uploadImage = new UploadImage();
        }

        public void OnGet(int id)
        {
            if (id.Equals(0))
            {
                _dataUserActualizar2 = null;
                _dataInput = null;
            }
            if (_dataInput != null || _dataUserActualizar1 != null || _dataUserActualizar2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.rolesLista = _usersRole.getRoles(_roleManager);
                    Input.AvatarImage = null;
                    Input.Image = _dataUserActualizar2.Image;
                }
                else
                {
                    if (_dataUserActualizar1 != null || _dataUserActualizar2 != null)
                    {
                        /*if (_dataUserActualizar2 != null)
                         {
                             _dataUserActualizar1 = _dataUserActualizar2;
                         }*/
                        Input = new inputModel
                        {
                            Id = _dataUserActualizar1.Id,
                            Name = _dataUserActualizar1.Name,
                            Apellido1 = _dataUserActualizar1.Apellido1,
                            Apellido2 = _dataUserActualizar1.Apellido2,
                            NID = _dataUserActualizar1.NID,
                            Email = _dataUserActualizar1.Email,
                            Image = _dataUserActualizar1.Image,
                            PhoneNumber = _dataUserActualizar1.IdentityUser.PhoneNumber,
                            rolesLista = getRoles(_dataUserActualizar1.Role)
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
                    rolesLista = _usersRole.getRoles(_roleManager)
                };
            }
            if (_dataUserActualizar2 == null)
            {
                _dataUserActualizar2 = _dataUserActualizar1;
            }
            _dataUserActualizar1 = null;
        }

        public async Task<IActionResult> OnPost(String dataUser, int id)
        {
            if (dataUser == null)
            {

                if (User.IsInRole("Administrador"))
                {


                    if (await DeleteAsync())
                    {
                        var url = $"/Users/Users/Users";
                        _dataUserActualizar2 = null;
                        return Redirect(url);
                    }
                    else
                    {
                        return Redirect("/Users/DeleteUser");
                    }
                }
                else
                {
                    return Redirect("/Home/ErrorNoAutorizado/");
                }
            }
            else
            {
                _dataUserActualizar1 = JsonConvert.DeserializeObject<InputModelRegister>(dataUser);
                return Redirect("/Users/DeleteUser/?id=" + id);
            }
        }



        private List<SelectListItem> getRoles(String role)
        {
            List<SelectListItem> rolesLista = new List<SelectListItem>();
            rolesLista.Add(new SelectListItem
            {
                Text = role
            });
            var roles = _usersRole.getRoles(_roleManager);
            roles.ForEach(item =>
            {
                if (item.Text != role)
                {
                    rolesLista.Add(new SelectListItem
                    {
                        Text = item.Text
                    });
                }
            });

            return rolesLista;
        }

        private async Task<bool> DeleteAsync()
        {
            var valor = false;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var identityUser = _userManager.Users.Where(u => u.Id.Equals(_dataUserActualizar2.ID)).ToList().Last();

                        if (_dataUserActualizar2.Role != Input.Role)
                        {
                            await _userManager.RemoveFromRoleAsync(identityUser, _dataUserActualizar2.Role);
                        }

                        _context.Remove(_context.TUsers.Single(u => u.ID == _dataUserActualizar2.Id));
                        await _context.SaveChangesAsync();

                        await _userManager.DeleteAsync(identityUser);
                        _context.SaveChanges();

                        transaction.Commit();
                        valor = true;
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                        valor = false;
                        throw;
                    }
                }

            });
            return valor;
        }

        [BindProperty]

        public inputModel Input { get; set; }

        public class inputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }

            public List<SelectListItem> rolesLista { get; set; }

        }

    }
}
