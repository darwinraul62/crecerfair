using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [Route("api/providers")]
    [ApiController]
    public partial class ProviderController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IFairRepositoryContainer repositoryContainer;
        private readonly IMapper mapper;
        public ProviderController(IFairRepositoryContainer repositoryContainer, IMapper mapper)
        {
            this.repositoryContainer = repositoryContainer;
            this.mapper = mapper;
        }

        [HttpGet]        
        [ProducesResponseType(typeof(QueryResult<ProviderViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] QueryRequest queryRequest)
        {
            var result = await repositoryContainer.Provider.GetAsync(queryRequest);

            var response = QueryResult.Convert<ProviderViewModelDTO>(result, 
                result.Data.ToDTO());

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProviderViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var provider = await repositoryContainer.Provider.GetFirstOrDefaultAsync(p => p.ProviderId == id);
            return Ok(provider.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProviderInsertResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Insert(ProviderInsertRequestDTO requestDTO)
        {
            if (await this.repositoryContainer.Provider.ExistsAsync(p => p.Identification == requestDTO.Identification))
                return ConflictModelResult("Ya existe un proveedor con la identificación ingresada");

            Data.Provider model = new Data.Provider();
            model.Identification = requestDTO.Identification;
            model.Active = requestDTO.Active;
            model.Address = requestDTO.Address;
            model.BusinessName = requestDTO.BusinessName;
            model.Email = requestDTO.Email;
            model.Website = requestDTO.Website;
            model.PhoneNumber1 = requestDTO.PhoneNumber1;
            model.PhoneNumber2 = requestDTO.PhoneNumber2;
            model.Tradename = requestDTO.Tradename;
            model.YoutubeAddress = requestDTO.YoutubeAddress;
            model.TwitterAddress = requestDTO.TwitterAddress;
            model.InstagramAddress = requestDTO.InstagramAddress;
            model.FacebookAddress = requestDTO.FacebookAddress;    
            model.CategoryId = requestDTO.CategoryId;        
            model.ProviderId = Guid.NewGuid();

            repositoryContainer.Provider.Add(model);
            await repositoryContainer.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.ProviderId }, new ProviderInsertResponseDTO
            {
                ProviderId = model.ProviderId
            });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update(ProviderUpdateRequestDTO requestDTO)
        {
            if (await this.repositoryContainer.Provider.ExistsAsync(p => p.ProviderId != p.ProviderId
                 && p.Identification == requestDTO.Identification))
                return ConflictModelResult("Ya existe un proveedor con la identificación ingresada");

            Data.Provider model = await this.repositoryContainer.Provider.GetFirstOrDefaultAsync(p => p.ProviderId == requestDTO.ProviderId);
            if (model == null)
                return NotFound("Proveedor no existe");

            model.Identification = requestDTO.Identification;
            model.Active = requestDTO.Active;
            model.Address = requestDTO.Address;
            model.BusinessName = requestDTO.BusinessName;
            model.Email = requestDTO.Email;
            model.Website = requestDTO.Website;
            model.PhoneNumber1 = requestDTO.PhoneNumber1;
            model.PhoneNumber2 = requestDTO.PhoneNumber2;
            model.Tradename = requestDTO.Tradename;
            model.YoutubeAddress = requestDTO.YoutubeAddress;
            model.TwitterAddress = requestDTO.TwitterAddress;
            model.InstagramAddress = requestDTO.InstagramAddress;
            model.FacebookAddress = requestDTO.FacebookAddress;
            model.CategoryId = requestDTO.CategoryId;

            repositoryContainer.Provider.Update(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

    }
}
