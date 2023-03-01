using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Admin.Web.Services
{
    public interface IProviderService
    {
        Task<QueryResult<ProviderViewModelDTO>> GetAsync(QueryRequest request);
        Task<ProviderViewModelDTO> GetAsync(Guid id);
        Task<ModelResult<ProviderInsertResponseDTO>> InsertAsync(ProviderInsertRequestDTO model);
        Task<ModelResult> UpdateAsync(ProviderUpdateRequestDTO model);
        Task<IEnumerable<ProviderResourceViewModelDTO>> GetResourcesAsync(Guid providerId, string resourceTypeId = null);
        Task<ProviderResourceViewModelDTO> GetResourceAsync(Guid providerId, Guid resourceId);        
        Task<ModelResult<ProviderResourceUpdateResponseDTO>> AddResourceAsync(Guid providerId, ProviderResourceGeneralUpdateRequestDTO request);
        Task UpdateResourceNameAsync(Guid providerId, Guid resourceId, string name);        
        Task UpdateResourcesPriorityAsync(Guid providerId, string type, List<ProviderResourceUpdatePriorityDTO> resources);
        Task<ModelResult> RemoveResourceAsync(Guid providerId, Guid resourceId);
        Task<IEnumerable<ProviderUserViewModelDTO>> GetUsersAsync(Guid providerId);
        Task<ModelResult> AddUsersAsync(Guid providerId, Guid userId);
        Task<ModelResult> RemoveUsersAsync(Guid providerId, Guid userId);
        Task<IEnumerable<ProviderRelationViewModelDTO>> GetProvidersByUser(Guid userId);
        Task<IEnumerable<ProviderCalendarViewModelDTO>> GetCalendarAsync(Guid providerId);
        Task<ModelResult> SetCalendarAsync(Guid providerId, IEnumerable<ProviderCalendarInsertRequestDTO> request);
    }
}
