using System;
using Autofac;
using Crecer.Fair.Data.Repositories;
using Crecer.Fair.Infrastructure;
using Crecer.Fair.Infrastructure.Repositories;

namespace Crecer.Fair.Api.Infrastructure.AutofacModules
{
    public class FairDataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<FairRepositoryContainer>()
                .As<IFairRepositoryContainer>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PartnertRepository>()
                .As<IPartnertRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProviderRepository>()
                .As<IProviderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProviderResourceRepository>()
                .As<IProviderResourceRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<ProviderCategoryRepository>()
                .As<IProviderCategoryRepository>()
                .InstancePerLifetimeScope();          

            builder.RegisterType<ProviderUserRepository>()
                .As<IProviderUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ResourceTypeRepository>()
                .As<IResourceTypeRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<FairRepository>()
                .As<IFairRepository>()
                .InstancePerLifetimeScope();           

            builder.RegisterType<FairStandRepository>()
                .As<IFairStandRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StandModelRepository>()
                .As<IStandModelRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<StandModelResourceRepository>()
                .As<IStandModelResourceRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StandVisitedRepository>()
                .As<IStandVisitedRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProviderCalendarRepository>()
                .As<IProviderCalendarRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PartnertStandVisitedResumeRepository>()
                .As<IPartnertStandVisitedResumeRepository>()
                .InstancePerLifetimeScope();   

            builder.RegisterType<FairUserRepository>()
                .As<IFairUserRepository>()
                .InstancePerLifetimeScope();                
        }
    }
}
