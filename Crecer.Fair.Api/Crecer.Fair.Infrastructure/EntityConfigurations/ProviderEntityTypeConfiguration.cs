using System;
using Crecer.Fair.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderEntityTypeConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.ToTable("provider");

            builder.HasKey(p => p.ProviderId);

            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
            builder.HasMany(p => p.Resources).WithOne(p => p.Provider).HasForeignKey(p => p.ProviderId);
            builder.HasMany(p => p.Users).WithOne(p => p.Provider);
            builder.HasMany(p => p.Calendar).WithOne(p => p.Provider).HasForeignKey(p => p.ProviderId);
        }
    }
}
