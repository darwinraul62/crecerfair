using System;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Infrastructure.EntityConfigurations;
using Ecubytes.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure
{
    public class FairDbContext : Ecubytes.Data.EntityFramework.DbContext
    {
        public FairDbContext(DbContextOptions<FairDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partnert> Partnerts { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderResource> ProviderResources { get; set; }
        public DbSet<ProviderUser> ProviderUsers { get; set; }
        public DbSet<ProviderCalendar> ProviderCalendar { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<ProviderCategory> Categories { get; set; }
        public DbSet<FairModel> Fairs { get; set; }
        public DbSet<FairStand> FairStands { get; set; }
        public DbSet<StandModel> StandModels { get; set; }
        public DbSet<StandModelResource> StandModelResources { get; set; }
        public DbSet<PartnertImportTemp> PartnertsImportTemp { get; set; }
        public DbSet<StandVisited> StandsVisited { get; set; }
        public DbSet<PartnertStandVisitedResume> PartnertStandVisitedResume { get; set; }

        public DbSet<ProviderVisitCount> ProviderVisitCount { get; set; }
        public DbSet<FairUsers> FairUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PartnertEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FairEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FairStandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StandModelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StandModelResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PartnertImportTempEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StandVisitedEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderVisitCountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderCalendarEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PartnertStandVisitedResumeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FairUsersEntityTypeConfiguration());

            modelBuilder.ToLowerCaseEntities();
        }

    }
}
