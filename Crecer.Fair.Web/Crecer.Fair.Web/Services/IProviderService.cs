using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Web.Services
{
    public interface IProviderService
    {
        Task<QueryResult<IEnumerable<ProviderViewModelDTO>>> GetAsync(QueryRequest request);
        Task<ProviderViewModelDTO> GetAsync(Guid id);
        Task<IEnumerable<ProviderResourceViewModelDTO>> GetResourcesAsync(Guid providerId, string resourceTypeId = null);
        Task<IEnumerable<ProviderCalendarViewModelDTO>> GetCalendarAsync(Guid providerId);
        Task<IEnumerable<ProviderUserViewModelDTO>> GetUsersAsync(Guid providerId);
    }
}
