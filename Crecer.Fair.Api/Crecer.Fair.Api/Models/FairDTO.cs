using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Models
{
    public class FairUpdateRequestDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string WelcomeText { get; set; }
        public Guid HostProviderId { get; set; }
    }
    public class FairViewModelDTO
    {
        public Guid FairId { get; set; }
        public string Title { get; set; }
        public string WelcomeText { get; set; }
        public Guid? HostProviderId { get; set; }
        public string HostProviderName { get; set; }
        public Guid? StandId { get; set; }
    }

    public static class FairModelConverter
    {
        public static IEnumerable<FairViewModelDTO> ToDTO(this IEnumerable<FairModel> model)
        {
            return model.Select(p => p.ToDTO());
        }

        public static FairViewModelDTO ToDTO(this FairModel model)
        {
            return new FairViewModelDTO()
            {
                FairId = model.FairId,
                Title = model.Title,
                WelcomeText = model.WelcomeText,
                HostProviderId = model.HostProviderId,
                HostProviderName = model.HostProvider?.Tradename
            };
        }
    }
}
