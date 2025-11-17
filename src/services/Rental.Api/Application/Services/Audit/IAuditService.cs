using Rental.Api.Entities.Audit;
using System;
using System.Threading.Tasks;

namespace Rental.Api.Application.Services.Audit
{
    public interface IAuditService
    {
        Task AddAsync(
            AuditEventType eventType,
            string message,
            object beforeState,
            object afterState);
    }
}
