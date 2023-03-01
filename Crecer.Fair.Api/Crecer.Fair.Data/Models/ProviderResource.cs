using System;
using System.Collections.Generic;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Data
{
    public class ProviderResource
    {
        public Guid ResourceId { get; set; }
        public Guid ProviderId { get; set; }
        public string Name { get; set; }
        public string ResourceTypeId { get; set; }
        public int Priority { get; set; }
        public string Value { get; set; }
        public string FileName { get; set; }
        public string ThumbnailPath { get; set; }
        public Provider Provider { get; set; }
        public ResourceType ResourceType { get; set; }
        //public List<ProviderResourceMedia> MediaResources { get; set;}
    }
}
