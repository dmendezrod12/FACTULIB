using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FactuLib.Models
{
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


    }
}
