using FactuLib.Areas.Clientes.Models;
using FactuLib.Data;
using FactuLib.Library;
using FactuLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Areas.Clientes.Pages.Account
{
    public class ReporteBusquedaClienteModel : PageModel
    {
        public static List<InputModelRegistrar> _dataclient;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private LCliente _cliente;

        public ReporteBusquedaClienteModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _context = context;
            _userManager = userManager;
            _cliente = new LCliente(context);
        }
        public IActionResult OnGet()
        {
            _dataclient = _cliente.getTClienteAsync(null, 0);

            Input = new InputModel
            {
                clientes = _dataclient    
            };
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public List<InputModelRegistrar> clientes;

        }

    }
}