using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using static Ecubytes.AspNetCore.Mvc.QueryRequestBinding.QueryRequestPropertiesNames;

namespace Crecer.Fair.Admin.Web.Services
{
    public class PartnertService : IPartnertService
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public PartnertService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile("Fair.Api");
        }

        public async Task<QueryResult<PartnertViewModelDTO>> GetAsync(QueryRequest queryRequest)
        {
            var response = await httpClient.GetAsync<QueryResult<PartnertViewModelDTO>>(APIUri.Partnert.Get(queryRequest));
            return response;
        }

        public async Task<ModelResult> ImportAsync(IEnumerable<PartnertImportRequestDTO> importRequestDTOs)
        {
            ModelResult modelResult = new ModelResult();
            var response = await httpClient.PostAsJsonAsync(APIUri.Partnert.Import(), importRequestDTOs);
            response.EnsureSuccessStatusCode();
            return modelResult;
        }
    }
}
