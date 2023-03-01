using System;

namespace Crecer.Fair.Data.Models
{
    public class PartnertStandVisitedResume
    {
        public Guid FairId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public Guid ProviderId { get; set; }
        public string Provider { get; set; }
        public Guid PartnertId { get; set; }
        public string Partnert { get; set; }
        public string PartnertIdentification { get; set; }
        public string PartnertEmail { get; set; }
        public string PartnertPhoneNumber { get; set; }
        public int VisitCount { get; set; }
    }
}
