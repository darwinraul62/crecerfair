using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Admin.Web.Services
{
    public interface IPartnertService
    {
        Task<QueryResult<PartnertViewModelDTO>> GetAsync(QueryRequest queryRequest);
        Task<ModelResult> ImportAsync(IEnumerable<PartnertImportRequestDTO> importRequestDTOs);
    }
}
