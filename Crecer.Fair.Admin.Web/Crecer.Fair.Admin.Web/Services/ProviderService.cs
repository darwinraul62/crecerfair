using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.JsonPatch;

namespace Crecer.Fair.Admin.Web.Services
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

        public async Task<ModelResult<ProviderResourceUpdateResponseDTO>> AddResourceAsync(Guid providerId, ProviderResourceGeneralUpdateRequestDTO request)
        {
            ModelResult<ProviderResourceUpdateResponseDTO> result = new ModelResult<ProviderResourceUpdateResponseDTO>();
            var response = await httpClient.PostAsJsonAsync(APIUri.Provider.AddResource(providerId),
                request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<ProviderResourceUpdateResponseDTO>();
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }
        public async Task UpdateResourcesPriorityAsync(Guid providerId, string type, List<ProviderResourceUpdatePriorityDTO> resources)
        {
            var response = await httpClient.PutAsJsonAsync(APIUri.Provider.UpdateResourcesPriority(providerId, type), resources);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateResourceNameAsync(Guid providerId, Guid resourceId, string name)
        {
            JsonPatchDocument<ProviderResourceGeneralUpdateRequestDTO> patchDocument = new JsonPatchDocument<ProviderResourceGeneralUpdateRequestDTO>();
            patchDocument.Replace(e => e.Name, name);

            var response = await httpClient.PacthAsJsonAsync(APIUri.Provider.UpdatePartial(providerId, resourceId), patchDocument);

            response.EnsureSuccessStatusCode();
        }


        public async Task<ModelResult> RemoveResourceAsync(Guid providerId, Guid resourceId)
        {
            ModelResult result = new ModelResult();

            var response = await httpClient.DeleteAsync(APIUri.Provider.DeleteResource(providerId, resourceId));

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }


        public Task<QueryResult<ProviderViewModelDTO>> GetAsync(QueryRequest request)
        {
            return httpClient.GetAsync<QueryResult<ProviderViewModelDTO>>(APIUri.Provider.Get(request));
        }

        public Task<ProviderViewModelDTO> GetAsync(Guid id)
        {
            return httpClient.GetAsync<ProviderViewModelDTO>(APIUri.Provider.Get(id));
        }

        public Task<ProviderResourceViewModelDTO> GetResourceAsync(Guid providerId, Guid resourceId)
        {
            return httpClient.GetAsync<ProviderResourceViewModelDTO>(APIUri.Provider.GetResource(providerId, resourceId));
        }

        public Task<IEnumerable<ProviderResourceViewModelDTO>> GetResourcesAsync(Guid providerId, string resourceTypeId = null)
        {
            return httpClient.GetAsync<IEnumerable<ProviderResourceViewModelDTO>>(APIUri.Provider.GetResources(providerId, resourceTypeId));
        }

        public async Task<ModelResult<ProviderInsertResponseDTO>> InsertAsync(ProviderInsertRequestDTO model)
        {
            ModelResult<ProviderInsertResponseDTO> result = new ModelResult<ProviderInsertResponseDTO>();
            var response = await httpClient.PostAsJsonAsync(APIUri.Provider.Insert(), model);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<ProviderInsertResponseDTO>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> UpdateAsync(ProviderUpdateRequestDTO model)
        {
            ModelResult result = new ModelResult();
            var response = await httpClient.PutAsJsonAsync(APIUri.Provider.Update(), model);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict
                || response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        #region Users

        public Task<IEnumerable<ProviderRelationViewModelDTO>> GetProvidersByUser(Guid userId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderRelationViewModelDTO>>(APIUri.Provider.GetProviderRelationByUser(userId));
        }

        public Task<IEnumerable<ProviderUserViewModelDTO>> GetUsersAsync(Guid providerId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderUserViewModelDTO>>(APIUri.Provider.GetUsers(providerId));
        }

        public async Task<ModelResult> AddUsersAsync(Guid providerId, Guid userId)
        {
            ModelResult result = new ModelResult();

            var response = await httpClient.PostAsJsonAsync(APIUri.Provider.AddUser(providerId), new ProviderAddUserRequestDTO()
            {
                UserId = userId
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> RemoveUsersAsync(Guid providerId, Guid userId)
        {
            ModelResult result = new ModelResult();

            var response = await httpClient.DeleteAsync(APIUri.Provider.RemoveUser(providerId, userId));

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        #endregion

        #region Calendar

        public Task<IEnumerable<ProviderCalendarViewModelDTO>> GetCalendarAsync(Guid providerId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderCalendarViewModelDTO>>(APIUri.Provider.GetCalendar(providerId));
        }

        public async Task<ModelResult> SetCalendarAsync(Guid providerId, IEnumerable<ProviderCalendarInsertRequestDTO> request)
        {
            ModelResult result = new ModelResult();

            var response = await httpClient.PostAsJsonAsync(APIUri.Provider.SetCalendar(providerId), request);

            response.EnsureSuccessStatusCode();

            return result;
        }

        #endregion
    }
}
