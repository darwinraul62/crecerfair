using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Web.Services
{
    public class ProviderService : IProviderService
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public ProviderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile("Fair.Api");
        }

        public Task<QueryResult<IEnumerable<ProviderViewModelDTO>>> GetAsync(QueryRequest request)
        {
            return httpClient.GetAsync<QueryResult<IEnumerable<ProviderViewModelDTO>>>(APIUri.Provider.Get(request));
        }

        public Task<ProviderViewModelDTO> GetAsync(Guid id)
        {
            return httpClient.GetAsync<ProviderViewModelDTO>(APIUri.Provider.Get(id));
        }
        public Task<IEnumerable<ProviderResourceViewModelDTO>> GetResourcesAsync(Guid providerId, string resourceTypeId = null)
        {
            return httpClient.GetAsync<IEnumerable<ProviderResourceViewModelDTO>>(APIUri.Provider.GetResources(providerId, resourceTypeId));
        }        
         public Task<IEnumerable<ProviderCalendarViewModelDTO>> GetCalendarAsync(Guid providerId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderCalendarViewModelDTO>>(APIUri.Provider.GetCalendar(providerId));
        }      
        public Task<IEnumerable<ProviderUserViewModelDTO>> GetUsersAsync(Guid providerId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderUserViewModelDTO>>(APIUri.Provider.GetUsers(providerId));
        }  
    }
}
