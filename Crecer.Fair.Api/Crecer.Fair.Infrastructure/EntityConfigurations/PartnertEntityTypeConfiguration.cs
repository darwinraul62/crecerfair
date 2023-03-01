using System;
using Crecer.Fair.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class PartnertEntityTypeConfiguration : IEntityTypeConfiguration<Partnert>
    {
        public void Configure(EntityTypeBuilder<Partnert> builder)
        {
            builder.ToTable("partnert");

            builder.HasKey(p=>p.PartnertId);
        }
    }
}
