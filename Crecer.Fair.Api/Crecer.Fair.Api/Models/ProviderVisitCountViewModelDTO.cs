using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Models
{
    public class ProviderVisitCountViewModelDTO
    {
        public Guid ProviderId { get; set; }
        public string Provider { get; set; }        
        public int VisitCount { get; set; }
    }

    public static class ProviderVisitCountModelConverter
    {
        public static IEnumerable<ProviderVisitCountViewModelDTO> ToDTO(this IEnumerable<ProviderVisitCount> model)
        {
            return model.Select(p=> p.ToDTO()).ToList();
        }
        public static ProviderVisitCountViewModelDTO ToDTO(this ProviderVisitCount model)
        {
            return new ProviderVisitCountViewModelDTO()
            {                
                Provider = model.Provider,
                ProviderId = model.ProviderId,
                VisitCount = model.VisitCount
            };
        }
    }
}
