using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class StandModelEntityTypeConfiguration : IEntityTypeConfiguration<StandModel>
    {
        public void Configure(EntityTypeBuilder<StandModel> builder)
        {
            builder.ToTable("standmodel");

            builder.HasKey(p => p.ModelCode);
            builder.HasMany(p => p.Resources).WithOne(p => p.StandModel).HasForeignKey(p => p.ModelCode);
        }
    }
}
