using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    public partial class FairController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        [HttpGet]
        [Route("{fairId}/stands")]
        [ProducesResponseType(typeof(IEnumerable<FairStandViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStands([FromRoute] Guid fairId)
        {
            List<FairStandViewModelDTO> modelDTO = await GetStandModelAsync(fairId);
            return Ok(modelDTO);
        }

        [HttpGet]
        [Route("{fairId}/stands/{standId}")]
        [ProducesResponseType(typeof(FairStandViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStandsById([FromRoute] Guid fairId, [FromRoute] Guid standId)
        {
            var modelDTO = (await GetStandModelAsync(fairId,standId)).FirstOrDefault();
            return Ok(modelDTO);
        }

        [HttpGet]
        [Route("{fairId}/stands/users")]
        [ProducesResponseType(typeof(IEnumerable<ProviderUserViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersStands([FromRoute] Guid fairId)
        {
            var stands = await repositoryContainer.FairStand.GetAsync();
            var listStands = stands.Select(p => p.ProviderId).Distinct().ToArray();

            var providerUsers = await repositoryContainer.ProviderUser.GetAsync(p=> listStands.Contains(p.ProviderId) );

            List<ProviderUserViewModelDTO> modelDTO = providerUsers.Select(p=> new ProviderUserViewModelDTO()
            {
                ProviderId = p.ProviderId,
                UserId = p.UserId
            }).ToList();
            
            return Ok(modelDTO);
        }

        private async Task<List<FairStandViewModelDTO>> GetStandModelAsync(Guid fairId, Guid? standId = null)
        {
            List<FairStandViewModelDTO> modelDTO = new List<FairStandViewModelDTO>();
            var queryRequest = QueryRequest.Builder();
            
            if(standId.HasValue)
                queryRequest.AddCondition("StandId", standId);

            var queryResult = await this.repositoryContainer.FairStand.GetAsync(queryRequest, p => p.FairId == fairId,
                includeProperties:"Provider.Category");
                        
            var model = queryResult.Data;

            if (model.Any())
            {
                Guid[] providerIds = model.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value).Distinct().ToArray();

                var resourcesLogo = await this.repositoryContainer.ProviderResource.GetAsync(p => p.ResourceTypeId == ResourceTypes.Logo
                   && providerIds.Contains(p.ProviderId));

                foreach (var item in model)
                {
                    modelDTO.Add(item.ToDTO(resourcesLogo.FirstOrDefault(p => p.ProviderId == item.ProviderId)));
                }
            }

            return modelDTO;
        }

        [HttpPatch]
        [Route("{fairId}/stands/{standId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateStand(
            [FromRoute] Guid fairId,
            [FromRoute] Guid standId,
            [FromBody] JsonPatchDocument<FairStandUpdateRequestDTO> patchDocument)
        {
            var model = await this.repositoryContainer.FairStand.GetFirstOrDefaultAsync(p => p.StandId == standId);

            if (model == null || fairId != model.FairId)
                return NotFound();

            var modelApi = mapper.Map<FairStandUpdateRequestDTO>(model);

            patchDocument.ApplyTo(modelApi);

            mapper.Map(modelApi, model);

            this.repositoryContainer.FairStand.Update(model);
            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("{fairId}/stands/{standId}/visits")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RegisterVisit(
            [FromRoute] Guid fairId,
            [FromRoute] Guid standId
        )
        {
            var standModel = await this.repositoryContainer.FairStand.GetFirstOrDefaultAsync(p => p.StandId == standId);

            if (standModel == null || fairId != standModel.FairId && standModel.ProviderId.HasValue)
                return NotFound();

            this.repositoryContainer.StandsVisited.Add(new Data.Models.StandVisited()
            {
                DateOfVisit = DateTime.UtcNow,
                LogId = Guid.NewGuid(),
                ProviderId = standModel.ProviderId.Value,
                StandId = standModel.StandId,
                UserId = Guid.Parse(User.FindFirstValue("sub"))
            });

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [Route("{fairId}/stands/count")]
        [ProducesResponseType(typeof(int),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStandsCount(
            [FromRoute] Guid fairId            
        )
        {
            int count = await this.repositoryContainer.FairStand.GetCountAsync(fairId);

            return Ok(count);
        }

        [HttpGet]
        [Authorize]
        [Route("{fairId}/stands/visits/count")]
        [ProducesResponseType(typeof(long),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStandsVisitCount(
            [FromRoute] Guid fairId,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            [FromQuery] Guid? providerId = null
        )
        {
            long count = await this.repositoryContainer.StandsVisited.VisitCountAsync(fairId, dateFrom, dateTo, providerId);

            return Ok(count);
        }

        [HttpGet]
        [Authorize]
        [Route("{fairId}/stands/visits/providers")]
        [ProducesResponseType(typeof(IEnumerable<ProviderVisitCountViewModelDTO>),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProviderVisitCount(
            [FromRoute] Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            Guid? providerId
        )
        {            
            var data = await this.repositoryContainer.StandsVisited.            
                GetProviderVisitCountAsync(fairId, dateFrom, dateTo, providerId);

            return Ok(data);
        }

        [HttpGet]
        [Authorize]
        [Route("{fairId}/stands/visits/partnerts/resume")]
        [ProducesResponseType(typeof(QueryResult<PartnertStandVisitedResumeDTO>),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetParnertResumeVisitCount(
            [FromRoute] Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            QueryRequest queryRequest
        )
        {            
            QueryResult<PartnertStandVisitedResumeDTO> result = null;

            var data = await this.repositoryContainer.PartnertStandVisitedResume.            
                GetResumeVisitCountAsync(fairId, dateFrom, dateTo, queryRequest);

            result = QueryResult.Convert<PartnertStandVisitedResumeDTO>(data, 
                data.Data.Select(p=> new PartnertStandVisitedResumeDTO()
            {
                Partnert = p.Partnert,
                PartnertId = p.PartnertId,
                PartnertIdentification = p.PartnertIdentification,
                PartnertEmail = p.PartnertEmail,
                PartnertPhoneNumber = p.PartnertPhoneNumber,
                Provider = p.Provider,
                ProviderId = p.ProviderId,
                VisitCount = p.VisitCount
            }));

            return Ok(result);
        }

    }
}