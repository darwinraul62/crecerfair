using System;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class ProviderResourceViewModelDTO
    {
        public Guid ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Value { get; set; }
        public string ResourceTypeName { get; set; }
        public string ThumbnailPath { get; set; }
        public int Priority { get; set; }
    }

    public class ProviderResourceGeneralUpdateRequestDTO
    {
        public Guid? ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        public string FileName { get; set; }
        public string ThumbnailPath { get; set; }
        public int Priority { get; set; }
    }

    public class ProviderResourceUpdateResponseDTO
    {
        public Guid ResourceId { get; set; }
    }
    public class ProviderResourceUpdatePriorityDTO
    {
        public Guid ResourceId { get; set; }
        public int Priority { get; set; }
    }
}