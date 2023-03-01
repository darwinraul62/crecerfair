using System;

namespace Crecer.Fair.Data.Models
{
    public class StandVisited
    {
        public Guid LogId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid StandId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public Provider Provider { get; set; }
        public FairStand FairStand { get; set; }
    }
}
