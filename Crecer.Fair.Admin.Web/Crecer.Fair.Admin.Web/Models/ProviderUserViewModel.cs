using System;

namespace Crecer.Fair.Admin.Web.Models
{
    public class ProviderUserViewModel
    {
        public Guid ProviderId { get; set; }
        public Guid UserId { get; set; }        
        public string UserLogonName { get; set; }
        public string UserFullName { get; set; }        
    }
}
