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
    public class ProviderCategoryService : IProviderCategoryService
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public ProviderCategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile("Fair.Api");
        }

        public Task<QueryResult<ProviderCategoryViewModelDTO>> GetAsync(QueryRequest request)
        {
            return httpClient.GetAsync<QueryResult<ProviderCategoryViewModelDTO>>(APIUri.ProviderCategory.Get(request));
        }

        public Task<ProviderCategoryViewModelDTO> GetAsync(Guid id)
        {
            return httpClient.GetAsync<ProviderCategoryViewModelDTO>(APIUri.ProviderCategory.Get(id));
        }
        
        public async Task<ModelResult<ProviderCategoryInsertResponseDTO>> InsertAsync(ProviderCategoryInsertRequestDTO model)
        {
            ModelResult<ProviderCategoryInsertResponseDTO> result = new ModelResult<ProviderCategoryInsertResponseDTO>();
            var response = await httpClient.PostAsJsonAsync(APIUri.ProviderCategory.Insert(), model);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<ProviderCategoryInsertResponseDTO>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> UpdateAsync(ProviderCategoryUpdateRequestDTO model)
        {
            ModelResult result = new ModelResult();
            var response = await httpClient.PutAsJsonAsync(APIUri.ProviderCategory.Update(), model);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict
                || response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.AddMessages(await response.Content.ReadAsModelResultAsync());
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }
    }
}