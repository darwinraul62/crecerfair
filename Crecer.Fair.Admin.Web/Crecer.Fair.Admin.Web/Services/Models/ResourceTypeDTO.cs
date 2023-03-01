using System;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class ResourceTypeViewModelDTO
    {
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool UserCanCreate { get; set; }
        public int Priority { get; set; }
    }    
}
