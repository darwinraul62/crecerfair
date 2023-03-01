using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderCalendarEntityTypeConfiguration : IEntityTypeConfiguration<ProviderCalendar>
    {
        public void Configure(EntityTypeBuilder<ProviderCalendar> builder)
        {
            builder.ToTable("providercalendar");

            builder.HasKey(p => p.Id);            
        }
    }
}
