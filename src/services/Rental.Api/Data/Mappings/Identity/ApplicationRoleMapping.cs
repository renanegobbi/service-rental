using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities.Identity;

namespace Rental.Api.Data.Mappings.Identity
{
    public class ApplicationRoleMapping : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("roles", "rental_service");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id");

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .HasMaxLength(256);

            builder.Property(r => r.NormalizedName)
                .HasColumnName("normalized_name")
                .HasMaxLength(256);

            builder.Property(r => r.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp");
        }
    }
}
