using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;

namespace Crecer.Fair.Admin.Web.Services
{
    public interface IResourceTypeService
    {
         Task<IEnumerable<ResourceTypeViewModelDTO>> GetAsync();
         Task<ResourceTypeViewModelDTO> GetAsync(string id);
    }
}
