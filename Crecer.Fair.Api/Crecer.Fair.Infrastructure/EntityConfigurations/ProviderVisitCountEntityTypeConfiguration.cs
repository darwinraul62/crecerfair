using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderVisitCountEntityTypeConfiguration : IEntityTypeConfiguration<ProviderVisitCount>
    {
        public void Configure(EntityTypeBuilder<ProviderVisitCount> builder)
        {
            builder.ToView("providervisitcount");

            builder.HasNoKey();
        }
    }
}
