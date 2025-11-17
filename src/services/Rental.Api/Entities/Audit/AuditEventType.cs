namespace Rental.Api.Entities.Audit
{
    /// <summary>
    /// Type of token used for authentication.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Portal token.
        /// </summary>
        Portal,

        /// <summary>
        /// Bearer JWT token.
        /// </summary>
        BearerJwt
    }

    /// <summary>
    /// Represents the type of event for audit logging.
    /// </summary>
    public enum AuditEventType
    {
        Created,
        Updated,
        Deleted,
        Error,
        Query
    }

    public static class AuditEventExtensions
    {
        public static string GetEventCode(this AuditEventType eventType)
        {
            return eventType switch
            {
                AuditEventType.Created => "CREATED",
                AuditEventType.Updated => "UPDATED",
                AuditEventType.Deleted => "DELETED",
                AuditEventType.Error => "ERROR",
                AuditEventType.Query => "QUERY",
                _ => string.Empty
            };
        }
    }
}
