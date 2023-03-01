using System;

namespace Crecer.Fair.Api.Models
{
    public class PartnertStandVisitedResumeDTO
    {
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
