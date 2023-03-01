using System;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data;

namespace Crecer.Fair.Web.Services
{
    public interface IPartnertService
    {
        Task<ModelResult<PartnertRegisterResponseDTO>> CreateAsync(PartnertRegisterRequestDTO requestModel);
        Task<ModelResult> UpdateAsync(Guid partnertId, PartnertRegisterRequestDTO requestModel);
        Task<PartnertViewModelDTO> GetByIdentificationAsync(string identification);
        Task<ModelResult> DeleteAsync(Guid partnertId);
        Task<bool> AllowRegisterAsync(string identification); 
    }
}
