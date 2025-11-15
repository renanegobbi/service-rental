using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rental.Api.Data.Mappings.Identity;
using Rental.Api.Entities.Identity;
using System;

namespace Rental.Api.Data
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("rental_service");

            builder.ApplyConfiguration(new ApplicationUserMapping());
            builder.ApplyConfiguration(new ApplicationRoleMapping());
            builder.ApplyConfiguration(new UserRoleMapping());
            builder.ApplyConfiguration(new UserClaimMapping());
            builder.ApplyConfiguration(new RoleClaimMapping());
            builder.ApplyConfiguration(new UserLoginMapping());
            builder.ApplyConfiguration(new RefreshTokenMapping());
        }
    }
}
