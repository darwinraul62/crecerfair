using System;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Identification { get; set; }
        [Required]
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }        
        public string Password { get; set; }        
        public string ConfirmPassword { get; set; }
        public string OtpEmailValidation { get; set; }
        public string Step { get; set; }
        public string Pwd { get; set; }
    }
}
