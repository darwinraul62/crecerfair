using System;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Authorization.Infrastructure
{
    public class AuthorizationDbContext : Ecubytes.Data.EntityFramework.DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
