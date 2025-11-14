using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Rental.Api.Data.Mappings.Identity
{
    public class RoleClaimMapping : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.ToTable("role_claims", "rental_service");

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.RoleId).HasColumnName("role_id");
            builder.Property(c => c.ClaimType).HasColumnName("claim_type");
            builder.Property(c => c.ClaimValue).HasColumnName("claim_value");
        }
    }
}
