using System;
using AutoMapper;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Api.Infrastructure.MappingProfiles
{
    public class FairStandProfile : Profile
    {
        public FairStandProfile()
        {
            CreateMap<FairStand,FairStandUpdateRequestDTO>().ReverseMap();
        }
    }
}
