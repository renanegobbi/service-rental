using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities;

namespace Rental.Api.Infrastructure.Mappings
{
    public class RentalPlanMapping : IEntityTypeConfiguration<RentalPlan>
    {
        public void Configure(EntityTypeBuilder<RentalPlan> builder)
        {
            builder.ToTable("rental_plan", "rental_service");

            builder.HasKey(rp => rp.Id);

            builder.Property(dlt => dlt.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            builder.Property(p => p.Days)
                   .IsRequired()
                   .HasColumnName("days");

            builder.Property(p => p.DailyRate)
                   .HasColumnType("numeric(10,2)")
                   .IsRequired()
                   .HasColumnName("daily_rate");

            builder.Property(p => p.PenaltyPercent)
                   .HasColumnType("numeric(5,2)")
                   .HasDefaultValue(0)
                   .HasColumnName("penalty_percent");

            builder.Property(p => p.Description)
                   .HasMaxLength(100)
                   .HasColumnName("description");

            builder.Property(p => p.CreatedAt)
                   .HasColumnType("timestamptz")
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(p => p.Days).HasDatabaseName("idx_plan_days");
        }
    }
}
