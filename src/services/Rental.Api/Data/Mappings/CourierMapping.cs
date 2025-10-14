using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities;

namespace Rental.Api.Data.Mappings
{
    public class CourierMapping : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasColumnName("full_name");

            builder.Property(c => c.Cnpj)
                .IsRequired()
                .HasColumnType("varchar(18)")
                .HasColumnName("cnpj");

            builder.Property(c => c.BirthDate)
                .IsRequired()
                .HasColumnType("date")
                .HasColumnName("birth_date");

            builder.Property(c => c.DriverLicenseNumber)
                .IsRequired()
                .HasColumnType("varchar(20)")
                .HasColumnName("driver_license_number");

            builder.Property(c => c.DriverLicenseType)
                .IsRequired()
                .HasColumnType("varchar(5)")
                .HasColumnName("driver_license_type");

            builder.Property(c => c.DriverLicenseImageUrl)
                .HasColumnType("text")
                .HasColumnName("driver_license_image_url");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(c => c.FullName)
                .HasDatabaseName("idx_courier_name");

            builder.HasIndex(c => c.Cnpj)
                .IsUnique()
                .HasDatabaseName("idx_courier_cnpj");

            builder.HasIndex(c => c.DriverLicenseNumber)
                .IsUnique()
                .HasDatabaseName("idx_courier_license_number");

            builder.HasCheckConstraint("chk_courier_license_type", "driver_license_type IN ('A', 'B', 'AB')");

            builder.ToTable(name: "courier", schema: "rental_service");
        }
    }
}
