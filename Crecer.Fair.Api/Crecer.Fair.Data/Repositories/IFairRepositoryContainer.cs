using System;
using Ecubytes.Data;

namespace Crecer.Fair.Data.Repositories
{
    public interface IFairRepositoryContainer : IUnitOfWork
    {
        IPartnertRepository Partnert { get; }
        IProviderRepository Provider { get; }
        IProviderCategoryRepository ProviderCategory { get; }
        IProviderUserRepository ProviderUser { get; }
        IProviderResourceRepository ProviderResource { get; }
        IResourceTypeRepository ResourceType { get; }
        IFairRepository Fair { get; }
        IFairStandRepository FairStand { get; }
        IStandModelRepository StandModel { get; }
        IStandModelResourceRepository StandModelResource { get; }
        IStandVisitedRepository StandsVisited { get; }
        IProviderCalendarRepository ProviderCalendar { get; }
        IPartnertStandVisitedResumeRepository PartnertStandVisitedResume { get; }
        IFairUserRepository FairUser { get; }
    }
}
