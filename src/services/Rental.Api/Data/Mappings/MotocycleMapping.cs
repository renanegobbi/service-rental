using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rental.Api.Entities;

namespace Rental.Api.Data.Mappings
{
    public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Year)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("year");

            builder.Property(m => m.Model)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("model");

            builder.Property(m => m.Plate)
                .IsRequired()
                .HasColumnType("varchar(10)")
                .HasColumnName("plate");

            builder.HasIndex(m => m.Plate)
                .IsUnique()
                .HasDatabaseName("idx_motorcycle_plate");

            builder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.ToTable(name: "motorcycle", schema: "rental_service");
        }
    }
}
