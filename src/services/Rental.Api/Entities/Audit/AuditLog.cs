using System;

namespace Rental.Api.Entities.Audit
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public string Message { get; set; }
        public string ObjectBefore { get; set; }
        public string ObjectAfter { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
        public Guid? CorrelationId { get; set; }
    }
}
