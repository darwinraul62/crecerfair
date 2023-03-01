using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
{
    public class ProviderCategoryViewModelDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class ProviderCategoryInsertRequestDTO
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
    public class ProviderCategoryInsertResponseDTO
    {
        public Guid CategoryId { get; set; }
    }
    public class ProviderCategoryUpdateRequestDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
    public static class ProviderCategoryModelConverter
    {
        public static IEnumerable<ProviderCategoryViewModelDTO> ToDTO(this IEnumerable<ProviderCategory> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static ProviderCategoryViewModelDTO ToDTO(this ProviderCategory model)
        {
            return new ProviderCategoryViewModelDTO()
            {
                Active = model.Active,
                CategoryId = model.CategoryId,
                Name = model.Name               
            };
        }
    }
}
