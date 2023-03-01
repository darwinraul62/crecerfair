using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crecer.Fair.Admin.Web.Models
{
    public class StandIndexViewModel
    {
        public Guid FairId { get; set; }
        public SelectList ProviderSelectList { get; set; }
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
        public bool IsEditable { get; set; }
        public string LogoUrl { get; set; }
        public int PositionRef { get; set; }
    }
}
