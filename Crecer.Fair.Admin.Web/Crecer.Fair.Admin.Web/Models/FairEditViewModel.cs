using System;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Admin.Web.Models
{
    public class FairEditViewModel
    {
        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Texto de Bienvenida")]
        public string WelcomeText { get; set; }
        [Display(Name = "Anfitrión")]
        public Guid? HostProviderId { get; set; }
        [Display(Name = "Anfitrión")]
        public string HostProviderName { get; set; }
    }
}
