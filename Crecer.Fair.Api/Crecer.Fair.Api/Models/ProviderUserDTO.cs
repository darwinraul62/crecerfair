using System;
using System.Collections.Generic;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
{
    public class ProviderUserViewModelDTO
    {
        public Guid ProviderId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ProviderAddUserRequestDTO
    {
        public Guid UserId { get; set; }
    }

    public static class ProviderUserViewModelConverter
    {
        public static IEnumerable<ProviderUserViewModelDTO> ToDTO(this IEnumerable<ProviderUser> data)
        {
            return data.Select(p => new ProviderUserViewModelDTO()
            {
                UserId = p.UserId
            });
        }
    }
}
