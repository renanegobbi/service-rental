using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities.Audit;
using Rental.Core.Data;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly RentalContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public AuditLogRepository(RentalContext context)
        {
            _context = context;
        }

        public async Task Add(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
        }
    }
}
