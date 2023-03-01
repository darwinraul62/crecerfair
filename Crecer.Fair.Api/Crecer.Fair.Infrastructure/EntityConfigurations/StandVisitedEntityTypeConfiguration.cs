using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class StandVisitedEntityTypeConfiguration : IEntityTypeConfiguration<StandVisited>
    {
        public void Configure(EntityTypeBuilder<StandVisited> builder)
        {
            builder.ToTable("standvisited");

            builder.HasKey(p => p.LogId);

            builder.HasOne(p => p.Provider).WithMany().HasForeignKey(p => p.ProviderId);
            builder.HasOne(p => p.FairStand).WithMany().HasForeignKey(p => p.StandId);
        }
    }
}
