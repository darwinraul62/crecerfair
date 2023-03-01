using System;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class FairUpdateRequestDTO
    {
        public string Title { get; set; }
        public string WelcomeText { get; set; }
        public Guid? HostProviderId { get; set; }
    }
    public class FairViewModelDTO
    {
        public Guid FairId { get; set; }
        public string Title { get; set; }
        public string WelcomeText { get; set; }
        public Guid? HostProviderId { get; set; }
        public string HostProviderName { get; set; }
    }

}
