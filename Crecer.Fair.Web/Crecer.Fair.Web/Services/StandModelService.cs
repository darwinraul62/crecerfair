using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;

namespace Crecer.Fair.Web.Services
{
    public class StandModelService : IStandModelService 
    {
        private const string PROFILENAME = "Fair.Api";
        private readonly HttpClient httpClient;
        public StandModelService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.SetProfile("Fair.Api");
        }

        public Task<StandModelViewModelDTO> GetAsync(string modelCode)
        {
            return httpClient.GetAsync<StandModelViewModelDTO>(APIUri.StandModel.Get(modelCode));
        }
    }
}
