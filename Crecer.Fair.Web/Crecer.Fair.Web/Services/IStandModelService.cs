using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;

namespace Crecer.Fair.Web.Services
{
    public interface IStandModelService
    {
        Task<StandModelViewModelDTO> GetAsync(string modelCode);
    }
}
