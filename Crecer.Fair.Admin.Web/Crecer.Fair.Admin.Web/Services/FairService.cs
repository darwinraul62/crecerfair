using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.JsonPatch;

namespace Crecer.Fair.Admin.Web.Services
{
    public class FairService : IFairService
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public FairService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile("Fair.Api");
        }
        public async Task<FairViewModelDTO> GetMainAsync()
        {
            var list = await httpClient.GetAsync<IEnumerable<FairViewModelDTO>>(APIUri.Fair.Get());
            return list.FirstOrDefault();
        }

        public Task<IEnumerable<FairStandViewModelDTO>> GetStandsAsync(Guid fairId)
        {
            return httpClient.GetAsync<IEnumerable<FairStandViewModelDTO>>(APIUri.Fair.GetStands(fairId));
        }

        public async Task UpdateAsync(Guid fairId, FairUpdateRequestDTO requestDTO)
        {
            var response = await httpClient.PutAsJsonAsync(APIUri.Fair.Update(fairId),requestDTO);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateStandProviderAsync(Guid fairId, Guid standId, Guid? providerId)
        {
            JsonPatchDocument<FairStandUpdateRequestDTO> patchDocument = new JsonPatchDocument<FairStandUpdateRequestDTO>();
            patchDocument.Replace(e => e.ProviderId, providerId);

            var response = await httpClient.PacthAsJsonAsync(APIUri.Fair.UpdateStandPartial(fairId,standId), patchDocument);

            response.EnsureSuccessStatusCode();
        }
        public Task<long> GetStandsVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId)
        {
            return httpClient.GetAsync<long>(APIUri.Fair.GetStandsVisitCount(fairId, dateFrom, dateTo, providerId));            
        }
        public Task<IEnumerable<ProviderVisitCountViewModelDTO>> GetProviderVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderVisitCountViewModelDTO>>(APIUri.Fair.GetProviderVisitCount(fairId, dateFrom, dateTo, providerId));
        }
        public Task<QueryResult<PartnertStandVisitedResumeDTO>> GetPartnertStandVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, QueryRequest queryRequest)
        {
            return httpClient.GetAsync<QueryResult<PartnertStandVisitedResumeDTO>>(APIUri.Fair.GetPartnertStandVisitCount(fairId, dateFrom, dateTo, queryRequest));
        }
        public Task<int> GetStandsCountAsync(Guid fairId)
        {
            return httpClient.GetAsync<int>(APIUri.Fair.GetStandsCount(fairId));            
        }
        public Task<QueryResult<FairUsersDTO>> GetFairsUsersAsync(QueryRequest request)
        {
            return httpClient.GetAsync<QueryResult<FairUsersDTO>>(APIUri.Fair.GetUserFairs(request));            
        }
    }
}
