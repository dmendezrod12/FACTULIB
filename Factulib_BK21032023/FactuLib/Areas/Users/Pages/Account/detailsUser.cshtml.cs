using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactuLib.Areas.Users.Models;
using FactuLib.Data;
using FactuLib.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FactuLib.Areas.Users.Pages.Account
{
    public class detailsUserModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private LUser _user;

        public detailsUserModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _user = new LUser(userManager, signInManager, roleManager, context);
        }
        public void OnGet(int id)
        {
            var data = _user.getTUsuariosAsync(null, id);
            if (0 < data.Result.Count)
            {
                Input = new inputModel {
                    DataUser = data.Result.ToList().Last(),
                };

            }
        }

        [BindProperty]

        public inputModel Input { get; set; }
        public class inputModel
        {
            public InputModelRegister DataUser { get; set; }
        }
    }
}
