using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class FairStandEntityTypeConfiguration : IEntityTypeConfiguration<FairStand>
    {
        public void Configure(EntityTypeBuilder<FairStand> builder)
        {
            builder.ToTable("fairstand");

            builder.HasKey(p => p.StandId);
            builder.HasOne(p => p.Provider).WithMany().HasForeignKey(p => p.ProviderId);
        }
    }
}
