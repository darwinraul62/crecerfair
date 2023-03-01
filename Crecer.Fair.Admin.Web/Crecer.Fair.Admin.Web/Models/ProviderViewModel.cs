using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crecer.Fair.Admin.Web.Models
{
    public class ProviderEditViewModel
    {        
        public Guid ProviderId { get; set; }
        [Required]
        [Display(Name = "Identificación")]
        public string Identification { get; set; }
        [Required]
        [Display(Name = "Nombre Comercial")]
        public string Tradename { get; set; }
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }
        [Display(Name = "Teléfono 1")]
        public string PhoneNumber1 { get; set; }
        [Display(Name = "Teléfono 2")]
        public string PhoneNumber2 { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Sitio WEB")]
        public string Website { get; set; }
        [Display(Name = "Facebook")]
        public string FacebookAddress { get; set; }
        [Display(Name = "Twitter")]
        public string TwitterAddress { get; set; }
        [Display(Name = "Youtube")]
        public string YoutubeAddress { get; set; }
        [Display(Name = "Instagram")]
        public string InstagramAddress { get; set; }
        [Display(Name = "Categoría")]
        public Guid? CategoryId { get; set; }

        public SelectList CategorySelectList {get; set;}

        [Required]
        [Display(Name = "Activo")]
        public bool Active { get; set; }
    }

    public class ProviderEditUsersViewModel
    {        
        public Guid ProviderId { get; set; }
        [Required]
        [Display(Name = "Identificación")]
        public string Identification { get; set; }
        [Required]
        [Display(Name = "Nombre Comercial")]
        public string Tradename { get; set; }        

        public SelectList SelectList {get; set;}        
    }
    
}
