using System;
using AutoMapper;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Infrastructure.MappingProfiles
{
    public class ProviderResourceProfile : Profile
    {
        public ProviderResourceProfile()
        {
            CreateMap<ProviderResource,ProviderResourceUpdateRequestDTO>().ReverseMap();
        }
    }
}
