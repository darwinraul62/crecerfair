using System;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure
{
    public class FairRepositoryContainer : RepositoryContainer<FairDbContext>, IFairRepositoryContainer
    {
        private readonly IServiceProvider serviceProvider;
        private IPartnertRepository partnertRepository;
        private IProviderRepository providerRepository;
        private IProviderResourceRepository providerResourceRepository;
        private IProviderUserRepository providerUserRepository;
        private IProviderCategoryRepository providerCategoryRepository;
        private IResourceTypeRepository resourceTypeRepository;
        private IFairRepository fairRepository;        
        private IFairStandRepository fairStandRepository;
        private IStandModelRepository standModelRepository;
        private IStandModelResourceRepository standModelResourceRepository;
        private IStandVisitedRepository standVisitedRepository;
        private IProviderCalendarRepository providerCalendarRepository;
        private IPartnertStandVisitedResumeRepository partnertStandVisitedResumeRepository;
        private IFairUserRepository fairUserRepository;
        public FairRepositoryContainer(FairDbContext Context,
            IServiceProvider serviceProvider)
            : base(Context)
        {
            this.serviceProvider = serviceProvider;
        }

        public IPartnertRepository Partnert => partnertRepository
            ?? (partnertRepository = (IPartnertRepository)serviceProvider.GetService(typeof(IPartnertRepository)));

        public IProviderRepository Provider => providerRepository
            ?? (providerRepository = (IProviderRepository)serviceProvider.GetService(typeof(IProviderRepository)));

        public IProviderResourceRepository ProviderResource => providerResourceRepository
            ?? (providerResourceRepository = (IProviderResourceRepository)serviceProvider.GetService(typeof(IProviderResourceRepository)));

        public IProviderCategoryRepository ProviderCategory => providerCategoryRepository
                    ?? (providerCategoryRepository = (IProviderCategoryRepository)serviceProvider.GetService(typeof(IProviderCategoryRepository)));

        public IProviderUserRepository ProviderUser => providerUserRepository
            ?? (providerUserRepository = (IProviderUserRepository)serviceProvider.GetService(typeof(IProviderUserRepository)));

        public IResourceTypeRepository ResourceType => resourceTypeRepository
            ?? (resourceTypeRepository = (IResourceTypeRepository)serviceProvider.GetService(typeof(IResourceTypeRepository)));
        
        public IFairRepository Fair => fairRepository
            ?? (fairRepository = (IFairRepository)serviceProvider.GetService(typeof(IFairRepository)));        
        
        public IFairStandRepository FairStand => fairStandRepository
            ?? (fairStandRepository = (IFairStandRepository)serviceProvider.GetService(typeof(IFairStandRepository)));
        public IStandModelRepository StandModel => standModelRepository
            ?? (standModelRepository = (IStandModelRepository)serviceProvider.GetService(typeof(IStandModelRepository)));
        public IStandModelResourceRepository StandModelResource => standModelResourceRepository
            ?? (standModelResourceRepository = (IStandModelResourceRepository)serviceProvider.GetService(typeof(IStandModelResourceRepository)));
        public IStandVisitedRepository StandsVisited => standVisitedRepository
            ?? (standVisitedRepository = (IStandVisitedRepository)serviceProvider.GetService(typeof(IStandVisitedRepository)));
        public IProviderCalendarRepository ProviderCalendar => providerCalendarRepository
            ?? (providerCalendarRepository = (IProviderCalendarRepository)serviceProvider.GetService(typeof(IProviderCalendarRepository)));

        public IPartnertStandVisitedResumeRepository PartnertStandVisitedResume => partnertStandVisitedResumeRepository
            ?? (partnertStandVisitedResumeRepository = (IPartnertStandVisitedResumeRepository)serviceProvider.GetService(typeof(IPartnertStandVisitedResumeRepository)));

        public IFairUserRepository FairUser => fairUserRepository
            ?? (fairUserRepository = (IFairUserRepository)serviceProvider.GetService(typeof(IFairUserRepository)));
    }
}