using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Models
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

    public static class ProviderCalendarViewModelConverter
    {
        public static IEnumerable<ProviderCalendarViewModelDTO> ToDTO(this IEnumerable<ProviderCalendar> data)
        {
            return data.Select(p => new ProviderCalendarViewModelDTO()
            {
                End = p.End,
                Id = p.Id,
                Start = p.Start,
                WeekDay = p.WeekDay
            });
        }
    }
}
