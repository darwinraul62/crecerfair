using System;

namespace Crecer.Fair.Data.Models
{
    public class ProviderCalendar
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }        
        public int WeekDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public Provider Provider { get; set; }
    }
}
