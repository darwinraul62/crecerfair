using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class PartnertStandVisitedResumeEntityTypeConfiguration : IEntityTypeConfiguration<PartnertStandVisitedResume>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PartnertStandVisitedResume> builder)
        {
            builder.ToView("partnerstandvisitedresume");
            builder.HasNoKey();
        }
    }
}
