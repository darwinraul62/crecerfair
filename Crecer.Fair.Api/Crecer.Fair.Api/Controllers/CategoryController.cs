using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IFairRepositoryContainer repositoryContainer;
        public CategoryController(IFairRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;            
        }

        [HttpGet]        
        [ProducesResponseType(typeof(QueryResult<ProviderCategoryViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] QueryRequest queryRequest)
        {
            var result = await repositoryContainer.ProviderCategory.GetAsync(queryRequest);

            var response = QueryResult.Convert<ProviderCategoryViewModelDTO>(result, 
                result.Data.ToDTO());

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProviderCategoryViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var model = await repositoryContainer.ProviderCategory.GetFirstOrDefaultAsync(p => p.CategoryId == id);
            return Ok(model.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProviderCategoryInsertResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Insert(ProviderCategoryInsertRequestDTO requestDTO)
        {
            if (await this.repositoryContainer.ProviderCategory.ExistsAsync(p => p.Name == requestDTO.Name))
                return ConflictModelResult("Ya existe una categoría con el mismo nombre");

            Data.ProviderCategory model = new Data.ProviderCategory();
            model.Name = requestDTO.Name;
            model.Active = requestDTO.Active;
            model.CategoryId = Guid.NewGuid();

            repositoryContainer.ProviderCategory.Add(model);
            await repositoryContainer.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.CategoryId }, new ProviderCategoryInsertResponseDTO
            {
                CategoryId = model.CategoryId
            });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update(ProviderCategoryUpdateRequestDTO requestDTO)
        {
            if (await this.repositoryContainer.ProviderCategory.ExistsAsync(p => p.CategoryId != p.CategoryId
                 && p.Name == requestDTO.Name))
                return ConflictModelResult("Ya existe una categoría con el mismo nombre");

            Data.ProviderCategory model = await this.repositoryContainer.ProviderCategory.GetFirstOrDefaultAsync(p => p.CategoryId == requestDTO.CategoryId);
            if (model == null)
                return NotFound("Categoría no existe");

            model.Name = requestDTO.Name;
            model.Active = requestDTO.Active;

            repositoryContainer.ProviderCategory.Update(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Data.ProviderCategory model = await this.repositoryContainer.ProviderCategory.GetFirstOrDefaultAsync(p => p.CategoryId == id);
            if (model == null)
                return NotFound("Categoría no existe");

            if(await repositoryContainer.Provider.ExistsAsync(p=>p.CategoryId == id))
                return ConflictModelResult("La Categoría esta en uso");

            repositoryContainer.ProviderCategory.Remove(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }
    }
}
