using FactuLib.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib.Library
{
    public class ListObject
    {
        public LUsersRoles _usersRole;
        public IdentityError _identityError;
        public ApplicationDbContext _context;
        public IWebHostEnvironment _environment;
        public RoleManager<IdentityRole> _roleManager;
        public UserManager <IdentityUser> _userManager;
        public SignInManager<IdentityUser> _signInManager;

    }


}
