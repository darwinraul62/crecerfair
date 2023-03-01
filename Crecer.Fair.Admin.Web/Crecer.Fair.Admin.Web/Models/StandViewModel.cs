using System;

namespace Crecer.Fair.Admin.Web.Models
{
    public class StandProviderViewModel
    {
        public Guid StandId { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid FairId { get; set; }
    }
}
