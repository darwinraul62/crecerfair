using System;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class ProviderCalendarViewModelDTO
    {
        public Guid Id { get; set; }
        public string WeekDayDescription { get; set; }
        public int WeekDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class ProviderCalendarInsertRequestDTO
    {
        public int WeekDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
