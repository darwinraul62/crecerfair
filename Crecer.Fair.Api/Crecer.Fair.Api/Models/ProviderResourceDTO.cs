using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
{
    public class ProviderResourceUpdateRequestDTO
    {
        public Guid? ResourceId { get; set; }
        [Required]
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string FileName { get; set; }
        public string ThumbnailPath { get; set; }
        public int Priority { get; set; }
        //public List<ProviderResourceMediaDTO> MediaResources { get; set; }
    }

    public class ProviderResourceUpdatePriorityDTO
    {
        public Guid ResourceId { get; set; }
        public int Priority { get; set; }        
    }

    // public class ProviderResourceMediaDTO
    // {
    //     public string FileName { get; set; }
    //     public string FullName { get; set; }
    //     public string Url { get; set; }
    //     public int Priority { get; set; }
    // }

    public class ProviderResourceUpdateResponseDTO
    {
        public Guid ResourceId { get; set; }
    }

    public class ProviderResourceViewModelDTO
    {
        public Guid ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Value { get; set; }
        public string ThumbnailPath { get; set; }
        public string ResourceTypeName { get; set; }
        public int Priority { get; set; }
    }

    public static class ProviderResourceModelConverter
    {
        public static List<ProviderResourceViewModelDTO> ToDTO(this IEnumerable<ProviderResource> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }

        public static ProviderResourceViewModelDTO ToDTO(this ProviderResource model)
        {
            return new ProviderResourceViewModelDTO()
            {
                ResourceTypeId = model.ResourceTypeId,
                Name = model.Name,
                FileName = model.FileName,
                Value = model.Value,
                ResourceId = model.ResourceId,
                ResourceTypeName = model.ResourceType?.Name,
                ThumbnailPath = model.ThumbnailPath,
                Priority = model.Priority
            };
        }
    }
}
