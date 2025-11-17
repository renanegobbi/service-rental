using Rental.Api.Data;
using Rental.Api.Entities.Audit;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Rental.Services.User;

namespace Rental.Api.Application.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly RentalContext _context;
        private readonly IUser _user;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public AuditService(RentalContext context, IUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task AddAsync(
            AuditEventType eventType,
            string message,
            object beforeState,
            object afterState)
        {
            try
            {
                var correlation = _user.GetCorrelationId();

                var audit = new AuditLog
                {
                    EventType = eventType.ToString(),
                    Message = message,
                    ObjectBefore = Serialize(beforeState),
                    ObjectAfter = Serialize(afterState),
                    Username = _user.IsAuthenticated() ? _user.GetUserEmail() : null,
                    CreatedAt = DateTime.UtcNow,
                    CorrelationId = string.IsNullOrEmpty(correlation) ? null : Guid.Parse(correlation)
                };

                _context.AuditLogs.Add(audit);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Failed to write audit log");
            }
        }

        private static string Serialize(object obj)
        {
            if (obj == null)
                return null;

            try
            {
                return JsonSerializer.Serialize(obj, _jsonOptions);
            }
            catch
            {
                return obj.ToString();
            }
        }
    }
}
