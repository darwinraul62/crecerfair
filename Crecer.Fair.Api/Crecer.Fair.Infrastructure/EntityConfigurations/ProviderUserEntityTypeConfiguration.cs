using System;
using Crecer.Fair.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crecer.Fair.Infrastructure.EntityConfigurations
{
    public class ProviderUserEntityTypeConfiguration : IEntityTypeConfiguration<ProviderUser>
    {
        public void Configure(EntityTypeBuilder<ProviderUser> builder)
        {
            builder.ToTable("provideruser");

            builder.HasKey(p => new { p.ProviderId, p.UserId });

            builder.HasOne(p => p.Provider).WithMany(p => p.Users).HasForeignKey(p => p.ProviderId);
        }
    }
}
