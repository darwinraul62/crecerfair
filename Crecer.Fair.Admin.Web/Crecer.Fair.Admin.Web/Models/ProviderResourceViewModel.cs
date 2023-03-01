using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crecer.Fair.Admin.Web.Models
{
    public class ProviderResourceIndexViewModel
    {
        public Guid ProviderId { get; set; }
        public string ProviderName { get; set; }
        public List<ResourceTypeViewModel> ResourceTypes { get; set; } = new List<ResourceTypeViewModel>();
    }

    public class ResourceTypeViewModel
    {
        public ProviderResourceIndexViewModel MainData { get; set; }
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool UserCanCreate { get; set; }
        public int Priority { get; set; }

        public List<ProviderResourceViewModel> Resources { get; set; } = new List<ProviderResourceViewModel>();
    }

    public class ProviderResourceViewModel
    {
        public Guid ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ResourceTypeName { get; set; }
        public string ThumbnailPath { get; set; }
        public string FileName { get; set; }
        public int Priority { get; set; }
    }

    public class ProviderResourceBaseEditViewModel
    {
        public Guid ProviderId { get; set; }
        public string ProviderName { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        public string ResourceTypeName { get; set; }
        public string ThumbnailPath { get; set; }
        [Display(Name = "Prioridad")]
        public int Priority { get; set; }
    }

    public class ProviderResourceGeneralEditViewModel : ProviderResourceBaseEditViewModel
    {
        public bool IsAjaxRequest { get; set; }
        public string Value { get; set; }
        public bool IsDelete { get; set; }
        public bool IsReadOnly { get; set; }
        public List<ProviderResourceValueViewModel> ResourceCollection { get; set; }
    }

    public class ProviderResourceValueViewModel
    {
        public Guid ResourceId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Priority { get; set; }
    }
}
