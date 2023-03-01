using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class PartnertImportTempEntityTypeConfiguration : IEntityTypeConfiguration<PartnertImportTemp>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PartnertImportTemp> builder)
        {
            builder.ToTable("partnertimporttemp");

            builder.HasKey(p => p.Id);
        }
    }
}
