using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [Route("api/fairs")]
    public partial class FairController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IFairRepositoryContainer repositoryContainer;
        private IMapper mapper;
        public FairController(IFairRepositoryContainer repositoryContainer, IMapper mapper)
        {
            this.repositoryContainer = repositoryContainer;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FairViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var fairsModel = await this.repositoryContainer.Fair.GetAsync(includeProperties: "HostProvider");
            List<FairViewModelDTO> fairViewModels = new List<FairViewModelDTO>();

            foreach (var f in fairsModel)
            {
                FairViewModelDTO model = new FairViewModelDTO()
                {
                    FairId = f.FairId,
                    HostProviderId = f.HostProviderId,
                    HostProviderName = f.HostProvider.Tradename,
                    Title = f.Title,
                    WelcomeText = f.WelcomeText
                };
                
                var stands = await this.repositoryContainer.FairStand.GetAsync(p => p.FairId == f.FairId && p.ProviderId == f.HostProviderId,
                    orderBy: p => p.OrderBy(o => o.PositionRef));                

                if (stands != null && stands.Any())
                {
                    model.StandId = stands.First().StandId;
                }

                fairViewModels.Add(model);
            }

            return Ok(fairViewModels);
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(FairViewModelDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var model = await this.repositoryContainer.Fair.GetFirstOrDefaultAsync(p => p.FairId == id, includeProperties: "HostProvider");

            if (model == null)
                return NotFound();

            return Ok(model.ToDTO());
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FairUpdateRequestDTO requestDTO)
        {
            var model = await this.repositoryContainer.Fair.GetFirstOrDefaultAsync(p => p.FairId == id);

            if (model == null)
                return NotFound();

            model.WelcomeText = requestDTO.WelcomeText;
            model.Title = requestDTO.Title;
            model.HostProviderId = requestDTO.HostProviderId;

            this.repositoryContainer.Fair.Update(model);
            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }
    }
}
