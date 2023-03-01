using System;

namespace Crecer.Fair.Web.Services.Models
{
    public class ProviderCalendarViewModelDTO
    {
        public Guid Id { get; set; }        
        public int WeekDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
