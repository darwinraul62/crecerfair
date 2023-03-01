using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Data;
using Ecubytes.Json;

namespace Crecer.Fair.Web.Services
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

        public async Task<ModelResult<PartnertRegisterResponseDTO>> CreateAsync(PartnertRegisterRequestDTO requestModel)
        {
            ModelResult<PartnertRegisterResponseDTO> result = new ModelResult<PartnertRegisterResponseDTO>();

            var response = await httpClient.PostAsJsonAsync(APIUri.Partnert.Create(), requestModel);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<PartnertRegisterResponseDTO>();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return result;
        }

        public async Task<bool> AllowRegisterAsync(string identification)
        {            
            return await httpClient.GetAsync<bool>(APIUri.Partnert.AllowRegister(identification));
        }
        public Task<PartnertViewModelDTO> GetByIdentificationAsync(string identification)
        {
            return httpClient.GetAsync<PartnertViewModelDTO>(APIUri.Partnert.GetByIdentification(identification));
        }

        public async Task<ModelResult> DeleteAsync(Guid partnertId)
        {
            ModelResult<PartnertRegisterResponseDTO> result = new ModelResult<PartnertRegisterResponseDTO>();

            var response = await httpClient.DeleteAsync(APIUri.Partnert.Delete(partnertId));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.Data = await response.Content.ReadAsAsync<PartnertRegisterResponseDTO>();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return result;
        }

        public async Task<ModelResult> UpdateAsync(Guid partnertId, PartnertRegisterRequestDTO requestModel)
        {
             ModelResult result = new ModelResult();

            var response = await httpClient.PutAsJsonAsync(APIUri.Partnert.Update(partnertId), requestModel);

            if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return result;
        }
    }
}