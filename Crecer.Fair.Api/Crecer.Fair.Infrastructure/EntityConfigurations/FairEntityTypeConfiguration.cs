using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class FairEntityTypeConfiguration : IEntityTypeConfiguration<FairModel>
    {
        public void Configure(EntityTypeBuilder<FairModel> builder)
        {
            builder.ToTable("fair");

            builder.HasKey(p => p.FairId);

            builder.HasOne(p => p.HostProvider).WithMany().HasForeignKey(p => p.HostProviderId);
            builder.HasMany(p => p.Stands).WithOne(p => p.Fair).HasForeignKey(p => p.FairId);
        }
    }
}
