using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class StandModelResourceEntityTypeConfiguration : IEntityTypeConfiguration<StandModelResource>
    {
        public void Configure(EntityTypeBuilder<StandModelResource> builder)
        {
            builder.ToTable("standmodelresource");

            builder.HasKey(p => new { p.ModelCode, p.ResourceTypeId });
        }
    }
}
