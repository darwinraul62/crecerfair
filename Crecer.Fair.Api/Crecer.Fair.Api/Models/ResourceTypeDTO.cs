using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
{
    public class ResourceTypeViewModelDTO
    {
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool UserCanCreate { get; set; }
        public int Priority { get; set; }
    }

    public static class ResourceTypeModelConverter
    {
        public static List<ResourceTypeViewModelDTO> ToDTO(this IEnumerable<ResourceType> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static ResourceTypeViewModelDTO ToDTO(this ResourceType model)
        {
            return new ResourceTypeViewModelDTO()
            {
                Active = model.Active,
                Name = model.Name,
                ResourceTypeId = model.ResourceTypeId,
                UserCanCreate = model.UserCanCreate, 
                Priority = model.Priority
            };
        }
    }
}
