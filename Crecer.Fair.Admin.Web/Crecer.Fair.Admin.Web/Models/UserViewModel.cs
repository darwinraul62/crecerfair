using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crecer.Fair.Admin.Web.Models
{
    public class UserEditViewModel
    {
        public Guid UserId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string Names { get; set; }
        [Display(Name = "Apellidos")]
        public string LastNames { get; set; }
        public bool BlockLogin { get; set; }
    }

    public class UserCreateViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Names { get; set; }
        public string LastNames { get; set; }
        public bool BlockLogin { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        [MinLength(8)]
        public string ConfirmPassword { get; set; }
    }

    public class UserEditGroupViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Nombre")]
        public string FullName { get; set; }
        [Display(Name = "Grupo")]
        public Guid? UserGroupId { get; set; }
        [Display(Name = "Grupo")]
        public string UserGroupName { get; set; }
        public SelectList UserGroupSelectList { get; set; }
    }

    public class UserReport
    {
        public Guid UserId { get; set; }
        public string LogonName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPartnert { get; set; }
        public bool IsProvider { get; set; }
        public string ProviderName { get; set; }
        public string UserGroupNames { get; set; }
        public bool BlockLogin { get; set; }
        public bool Online { get; set; }
        public DateTime? LastAccess { get; set; }
        public string LastAccessString { get; set; }

    }
}
