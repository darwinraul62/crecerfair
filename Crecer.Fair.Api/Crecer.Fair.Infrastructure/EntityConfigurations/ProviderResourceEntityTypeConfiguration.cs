using System;
using Crecer.Fair.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderResourceEntityTypeConfiguration : IEntityTypeConfiguration<ProviderResource>
    {
        public void Configure(EntityTypeBuilder<ProviderResource> builder)
        {
            builder.ToTable("providerresource");

            builder.HasKey(p => p.ResourceId);

            builder.HasOne(p => p.ResourceType).WithMany().HasForeignKey(p => p.ResourceTypeId);

            //builder.HasMany(p => p.MediaResources).WithOne().HasForeignKey(p => p.Resource);

            //builder.HasOne(p => p.Provider).WithMany().HasForeignKey(p => p.ProviderId);
        }
    }
}
