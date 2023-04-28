using FactuLib.Areas.Principal.Controllers;
using FactuLib.Areas.Users.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Controllers
{
    public class HomeController : Controller
    {
        //IServiceProvider _serviceProvider;

        private static InputModelLogin _model;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LUser _user;
        private SignInManager<IdentityUser> _signInManager;
        private static RecoveryPassViewModel _recoveryPassModel;
        
        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            Data.ApplicationDbContext context,
            IServiceProvider serviceProvider)
        {
            //_serviceProvider = serviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _user = new LUser(userManager, signInManager, roleManager, context);
        }

        public async Task<IActionResult> Index()
        {

            //await createRolesAsync(_serviceProvider);
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
            }
            else
            {
                if (_model != null)
                {
                    return View(_model);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputModelLogin model)
        {
            _model = model;

            if (ModelState.IsValid)
            {
                var result = await _user.UserLoginAsync(model);
                if (result.Succeeded)
                {
                    return Redirect("/Principal/Principal");
                }
                else
                {
                    _model.ErrorMessage = "Correo o contraseña inválidos.";
                    return Redirect("/");
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _model.ErrorMessage = error.ErrorMessage;
                    }
                }
                return Redirect("/");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task createRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            String[] rolesName = { "Administrador", "Usuario Final" };

            foreach (var item in rolesName)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
        }

        public ActionResult ErrorNoAutorizado()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StartRecovery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartRecovery(RecoveryViewModel model)
        {
            try
            {
                LSendMails envio = new LSendMails();
                LHash encr = new LHash();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string token = encr.GetSha256(Guid.NewGuid().ToString());

                using (_context = new ApplicationDbContext())
                {
                    var Ouser = _context.TUsers.Where(d => d.Email == model.Email).FirstOrDefault();
                    if (Ouser == null)
                    {
                        model.ErrorMessage = "El correo electronico ingresado no esta registrado en el sistema";
                        return View(model);
                    }
                    if (Ouser != null)
                    {
                        Ouser.Token = token;
                        _context.Update(Ouser);
                        _context.SaveChanges();

                        //Enviar Correo

                        envio.SendEmail(Ouser.Email,token);



                    }
                }

                return View("StartRecoveryConfirmar");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }       
        }

        [HttpGet]
        public ActionResult Recovery(String token)
        {
            RecoveryPassViewModel model = new RecoveryPassViewModel();
            model.token = token;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Recovery(RecoveryPassViewModel model)
        {

            LHash encr = new LHash();

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                using (_context = new ApplicationDbContext())
                {
                    var oUser = _context.TUsers.Where(d => d.Token == model.token).FirstOrDefault();

                    if (oUser == null)
                    {
                        model.ErrorMessage = "NO SE REALIZO CAMBIO DE CONTRASEÑA: El token ya no es valido, vuelva a solicitar un nuevo correo de cambio de contraseña";
                        return View(model);
                    }
                    var oUserASP = _context.Users.Where(d => d.Id == oUser.IdUser).FirstOrDefault();
                    var identityUser = _userManager.Users.Where(u => u.Id.Equals(oUserASP.Id)).ToList().Last();

                    

                    if (oUserASP != null)
                    {
                        //string genToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                        //await _userManager.ResetPasswordAsync(identityUser,genToken,model.Password);

                       var result2 = await _userManager.RemovePasswordAsync(identityUser);
                       var result = await _userManager.AddPasswordAsync(identityUser, model.Password);
                       
                    }
                }

                string token = encr.GetSha256(Guid.NewGuid().ToString());

                using (_context = new ApplicationDbContext())
                {
                    var Ouser = _context.TUsers.Where(d => d.Token == model.token).FirstOrDefault();
                    if (Ouser != null)
                    {
                        Ouser.Token = token;
                        _context.Update(Ouser);
                        _context.SaveChanges();
                    }
                }

                ViewBag.Message = "Contraseña Modificada con exito";
                return View("Index");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
   
        }
    }
}
