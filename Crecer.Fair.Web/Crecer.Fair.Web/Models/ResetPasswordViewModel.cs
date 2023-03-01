using System;

namespace Crecer.Fair.Web.Models
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Otp { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set;}
    }
}
