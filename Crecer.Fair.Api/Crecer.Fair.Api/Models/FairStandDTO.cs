using System;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Models
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
        public string ModelCode { get; set; }
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
        public string ModelCode { get; set; }
        public bool IsEditable { get; set; }
    }

    public static class FairStandModelConverter
    {
        public static FairStandViewModelDTO ToDTO(this FairStand model, ProviderResource logo)
        {
            return new FairStandViewModelDTO()
            {
                FairId = model.FairId,
                Height = model.Height,
                LogoUrl = logo?.Value,
                ProviderId = model.ProviderId,
                ProviderName = model.Provider?.Tradename,
                StandId = model.StandId,
                Width = model.Width,
                ModelCode = model.ModelCode,
                IsEditable = model.IsEditable,
                ProviderCategoryId = model.Provider?.Category?.CategoryId,
                ProviderCategoryName = model.Provider?.Category?.Name,
                PositionRef = model.PositionRef,
                X = model.X,
                Y = model.Y
            };
        }
    }
}