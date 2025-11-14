using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Rental.Api.Data.Mappings.Identity
{
    public class UserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.ToTable("user_roles", "rental_service");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Property(ur => ur.UserId).HasColumnName("user_id");
            builder.Property(ur => ur.RoleId).HasColumnName("role_id");
        }
    }
}
