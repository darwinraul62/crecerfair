using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Admin.Web.Models
{
    public class ProviderCalendarEditViewModel
    {
        public Guid ProviderId { get; set; }
        public string Identification { get; set; }
        public string Tradename { get; set; }
        public List<ProviderCalendarItemEditViewModel> CalendarItems { get; set; }
    }

    public class ProviderCalendarItemEditViewModel
    {
        public int WeekDay { get; set; }
        public string WeekDayDescription { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
