using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Models
{
    public class StandModelViewModelDTO
    {
        public string ModelCode { get; set; }
        public string Name { get; set; }
        public List<StandModelResourceViewModelDTO> Resources { get; set; }
    }
    public class StandModelResourceViewModelDTO
    {        
        public string ResourceTypeId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static class StandModelModelConverter
    {
        public static StandModelViewModelDTO ToDTO(this StandModel model)
        {
            return new StandModelViewModelDTO()
            {
                ModelCode = model.ModelCode,
                Name = model.Name,
                Resources = model.Resources?.Select(p=> new StandModelResourceViewModelDTO()
                {
                    Height = p.Height,
                    ResourceTypeId = p.ResourceTypeId,
                    Width = p.Width,
                    X = p.X,
                    Y = p.Y
                }).ToList()
            };
        }
    }
}
