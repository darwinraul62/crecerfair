using System;

namespace Crecer.Fair.Data.Models
{
    public class ProviderVisitCount
    {
        public Guid FairId { get; set; }
        public Guid ProviderId { get; set; }
        public string Provider { get; set; }
        public DateTime DateOfVisit { get; set; }
        public int VisitCount { get; set; }
    }
}
