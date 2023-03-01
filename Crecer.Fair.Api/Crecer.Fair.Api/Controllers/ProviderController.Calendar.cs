using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    public partial class ProviderController
    {
        [HttpGet]
        [Route("{providerId}/calendar")]
        [ProducesResponseType(typeof(IEnumerable<ProviderCalendarViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCalendar([FromRoute] Guid providerId)
        {
            var data = await this.repositoryContainer.ProviderCalendar.GetAsync(p => p.ProviderId == providerId);
            return Ok(data.ToDTO());
        }

        [HttpPost]
        [Route("{providerId}/calendar")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PostCalendar(
            [FromRoute] Guid providerId,
            [FromBody] IEnumerable<ProviderCalendarInsertRequestDTO> requestInsert)
        {
            var dataExists = await this.repositoryContainer.ProviderCalendar.GetAsync(p => p.ProviderId == providerId);
            
            if(dataExists.Any())
                this.repositoryContainer.ProviderCalendar.RemoveRange(dataExists);

            List<ProviderCalendar> modelInsert = new List<ProviderCalendar>();
            modelInsert = requestInsert.Select(p=> new ProviderCalendar()
            {
                End = p.End,
                Id = Guid.NewGuid(),
                ProviderId = providerId,
                Start = p.Start,
                WeekDay = p.WeekDay
            }).ToList();
            
            repositoryContainer.ProviderCalendar.AddRange(modelInsert);
            
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

    }
}
