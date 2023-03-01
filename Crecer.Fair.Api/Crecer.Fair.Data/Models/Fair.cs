using System;
using System.Collections.Generic;

namespace Crecer.Fair.Data.Models
{
    public class FairModel
    {
        public Guid FairId { get; set; }
        public Guid? HostProviderId { get; set; }
        public string Title { get; set; }
        public string WelcomeText { get; set; }
        public Provider HostProvider { get; set; }
        public List<FairStand> Stands { get; set; }
    }
}
