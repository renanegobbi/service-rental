using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities;

namespace Rental.Api.Infrastructure.Mappings
{
    public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("motorcycle", "rental_service");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(m => m.Year)
                .IsRequired()
                .HasColumnName("year");

            builder.Property(m => m.Model)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("model");

            builder.Property(m => m.Plate)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("plate");

            builder.Property(m => m.CreatedAt)
                .HasColumnType("timestamptz")
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(m => m.Model)
                   .HasDatabaseName("idx_motorcycle_model");

            builder.HasIndex(m => m.Plate)
                   .IsUnique()
                   .HasDatabaseName("ux_motorcycle_plate");
        }
    }
}
