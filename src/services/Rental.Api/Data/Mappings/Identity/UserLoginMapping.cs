using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Rental.Api.Data.Mappings.Identity
{
    public class UserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        {
            builder.ToTable("user_logins", "rental_service");

            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Property(l => l.LoginProvider).HasColumnName("login_provider");
            builder.Property(l => l.ProviderKey).HasColumnName("provider_key");
            builder.Property(l => l.ProviderDisplayName).HasColumnName("provider_display_name");
            builder.Property(l => l.UserId).HasColumnName("user_id");
        }
    }
}
