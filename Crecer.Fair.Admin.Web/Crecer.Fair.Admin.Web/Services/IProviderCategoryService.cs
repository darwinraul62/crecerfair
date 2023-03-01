using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Admin.Web.Services
{
    public interface IProviderCategoryService
    {
        Task<QueryResult<ProviderCategoryViewModelDTO>> GetAsync(QueryRequest request);
        Task<ProviderCategoryViewModelDTO> GetAsync(Guid id);
        Task<ModelResult<ProviderCategoryInsertResponseDTO>> InsertAsync(ProviderCategoryInsertRequestDTO model);
        Task<ModelResult> UpdateAsync(ProviderCategoryUpdateRequestDTO model);
    }
}
