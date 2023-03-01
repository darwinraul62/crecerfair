using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [ApiController]
    [Route("api/resourceTypes")]
    public class ResourceTypeController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IFairRepositoryContainer repositoryContainer;
        public ResourceTypeController(IFairRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;               
        }

        [HttpGet]        
        [ProducesResponseType(typeof(IEnumerable<ResourceTypeViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var model = await this.repositoryContainer.ResourceType.GetAsync();
            return Ok(model.ToDTO());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResourceTypeViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var model = await this.repositoryContainer.ResourceType.GetFirstOrDefaultAsync(p=>p.ResourceTypeId == id);
            return Ok(model.ToDTO());
        }
    }
}