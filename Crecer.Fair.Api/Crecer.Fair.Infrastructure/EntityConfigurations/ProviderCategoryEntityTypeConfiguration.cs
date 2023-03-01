using System;
using Crecer.Fair.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProviderCategory>
    {
        public void Configure(EntityTypeBuilder<ProviderCategory> builder)
        {
            builder.ToTable("providercategory");

            builder.HasKey(p => p.CategoryId);
        }
    }
}
