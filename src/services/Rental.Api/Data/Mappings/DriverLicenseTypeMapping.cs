using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities;

namespace Rental.Api.Data.Mappings
{
    public class DriverLicenseTypeMapping : IEntityTypeConfiguration<DriverLicenseType>
    {
        public void Configure(EntityTypeBuilder<DriverLicenseType> builder)
        {
            builder.HasKey(dlt => dlt.Id);

            builder.Property(dlt => dlt.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(dlt => dlt.Code)
                .IsRequired()
                .HasColumnType("varchar(8)")
                .HasColumnName("code");

            builder.Property(dlt => dlt.Description)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasColumnName("description");

            builder.Property(dlt => dlt.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(dlt => dlt.Code)
                .HasDatabaseName("idx_license_type_code");

            builder.ToTable(name: "driver_license_type", schema: "rental_service");
        }
    }
}
