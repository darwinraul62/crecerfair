using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;

namespace Crecer.Fair.Admin.Web.Services
{
    public class ResourceTypeService : IResourceTypeService
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public ResourceTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile(PROFILENAME);
        }
      
        public Task<IEnumerable<ResourceTypeViewModelDTO>> GetAsync()
        {           
            return httpClient.GetAsync<IEnumerable<ResourceTypeViewModelDTO>>(APIUri.ResourceType.Get());
        }

        public Task<ResourceTypeViewModelDTO> GetAsync(string id)
        {
            return httpClient.GetAsync<ResourceTypeViewModelDTO>(APIUri.ResourceType.Get(id));
        }
    }
}
