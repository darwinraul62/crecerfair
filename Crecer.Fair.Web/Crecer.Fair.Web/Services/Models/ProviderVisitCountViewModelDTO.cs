using System;

namespace Crecer.Fair.Web.Services.Models
{
    public class ProviderVisitCountViewModelDTO
    {
        public Guid ProviderId { get; set; }
        public string Provider { get; set; }
        public DateTime DateOfVisit { get; set; }
        public int VisitCount { get; set; }
    }
}
