using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rental.Api.Entities.Audit;

namespace Rental.Api.Data.Mappings.Audit
{
    public class AuditMapping : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("audit_log", "rental_service");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id");

            builder.Property(x => x.EventType)
                   .HasColumnName("event_type");

            builder.Property(x => x.Message)
                   .HasColumnName("message");

            builder.Property(x => x.ObjectBefore)
                   .HasColumnName("object_before");

            builder.Property(x => x.ObjectAfter)
                   .HasColumnName("object_after");

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("NOW()");

            builder.Property(x => x.Username)
                   .HasColumnName("username");

            builder.Property(x => x.CorrelationId)
                   .HasColumnName("correlation_id")
                   .HasColumnType("uuid")
                   .IsRequired(false);
        }
    }
}
