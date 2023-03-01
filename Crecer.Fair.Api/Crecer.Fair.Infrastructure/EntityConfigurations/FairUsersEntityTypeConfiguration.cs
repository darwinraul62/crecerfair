using System;
using Crecer.Fair.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class FairUsersEntityTypeConfiguration : IEntityTypeConfiguration<FairUsers>
    {
        public void Configure(EntityTypeBuilder<FairUsers> builder)
        {
            builder.ToView("fairusers");

            builder.HasNoKey();
        }
    }
}
