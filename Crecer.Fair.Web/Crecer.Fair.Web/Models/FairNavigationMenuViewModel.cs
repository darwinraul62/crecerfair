using System;
using System.Collections.Generic;

namespace Crecer.Fair.Web.Models
{
    public record FairNavigationMenuViewModel
    {
        public List<StandProviderCategory> Categories { get; set; }
    }

    public record StandProviderCategory
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public List<StandProviderNav> Stands { get; set; }
    }

    public record StandProviderNav
    {
        public Guid StandId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int PositionRef { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
