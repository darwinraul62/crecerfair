using System;

namespace Crecer.Fair.Admin.Web.Services.Models
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
}
