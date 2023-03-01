using System;
using System.Collections.Generic;

namespace Crecer.Fair.Web.Models
{
    public class StandIndexViewModel
    {
        public Guid FairId { get; set; }        
        public List<StandCoordViewModel> Stands { get; set; } = new List<StandCoordViewModel>();
    }

    public class StandCoordViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ProviderName { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid StandId { get; set; }
        public string LogoUrl { get; set; }
    }
}
