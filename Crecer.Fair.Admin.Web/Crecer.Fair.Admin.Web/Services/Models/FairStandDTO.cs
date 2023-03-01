using System;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class FairStandViewModelDTO
    {
        public Guid StandId { get; set; }
        public Guid FairId { get; set; }
        public Guid? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string LogoUrl { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsEditable { get; set; }
        public int PositionRef { get; set; }
        public Guid? ProviderCategoryId { get; set; }
        public string ProviderCategoryName { get; set; }
    }
    public class FairStandUpdateRequestDTO
    {
        public Guid? ProviderId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
