using System;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Web.Models
{
    public class PasswordRecoveryRequestViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
