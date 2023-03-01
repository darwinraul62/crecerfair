using System;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string LogonName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string Identification { get; set; }
        public bool IsRegister { get; set; }
    }
}
