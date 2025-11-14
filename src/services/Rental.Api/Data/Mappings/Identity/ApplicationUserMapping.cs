using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities.Identity;

namespace Rental.Api.Data.Mappings.Identity
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users", "rental_service");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(u => u.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(256);

            builder.Property(u => u.NormalizedUserName)
                .HasColumnName("normalized_username")
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(256);

            builder.Property(u => u.NormalizedEmail)
                .HasColumnName("normalized_email")
                .HasMaxLength(256);

            builder.Property(u => u.EmailConfirmed)
                .HasColumnName("email_confirmed");

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(u => u.SecurityStamp)
                .HasColumnName("security_stamp");

            builder.Property(u => u.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp");

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number");

            builder.Property(u => u.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed");

            builder.Property(u => u.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled");

            builder.Property(u => u.LockoutEnd)
                .HasColumnName("lockout_end");

            builder.Property(u => u.LockoutEnabled)
                .HasColumnName("lockout_enabled");

            builder.Property(u => u.AccessFailedCount)
                .HasColumnName("access_failed_count");
        }
    }
}
