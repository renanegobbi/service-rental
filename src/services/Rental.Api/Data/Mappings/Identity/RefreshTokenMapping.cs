using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities.Identity;

namespace Rental.Api.Data.Mappings.Identity
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens", "rental_service");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id");

            builder.Property(r => r.Username)
                .HasColumnName("username");

            builder.Property(r => r.Token)
                .HasColumnName("token");

            builder.Property(r => r.ExpirationDate)
                .HasColumnName("expiration_date");
        }
    }
}
