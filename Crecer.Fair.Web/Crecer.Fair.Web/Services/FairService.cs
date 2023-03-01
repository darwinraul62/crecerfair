using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Web.Services
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
        public Task<FairStandViewModelDTO> GetStandByIdAsync(Guid fairId, Guid standId)
        {
            return httpClient.GetAsync<FairStandViewModelDTO>(APIUri.Fair.GetStandsById(fairId, standId));
        }
        public Task<IEnumerable<ProviderUserViewModelDTO>> GetStandsUsersAsync(Guid fairId)
        {
            return httpClient.GetAsync<IEnumerable<ProviderUserViewModelDTO>>(APIUri.Fair.GetStandsUsers(fairId));
        }
        public async Task RegisterStandVisit(Guid fairId, Guid standId)
        {
            var response = await httpClient.PostAsync(APIUri.Fair.RegisterStandVisit(fairId, standId), null);
            response.EnsureSuccessStatusCode();
        }       
    }
}
