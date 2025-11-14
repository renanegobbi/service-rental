using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Rental.Api.Data.Mappings.Identity
{
    public class UserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.ToTable("user_claims", "rental_service");

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.UserId).HasColumnName("user_id");
            builder.Property(c => c.ClaimType).HasColumnName("claim_type");
            builder.Property(c => c.ClaimValue).HasColumnName("claim_value");
        }
    }
}
