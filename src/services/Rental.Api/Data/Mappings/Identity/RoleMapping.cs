using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Rental.Api.Data.Mappings.Identity
{
    public class RoleMapping : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.ToTable("roles", "rental_service");

            builder.Property(r => r.Name)
                .HasColumnName("name");

            builder.Property(r => r.NormalizedName)
                .HasColumnName("normalized_name");

            builder.Property(r => r.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp");
        }
    }
}
