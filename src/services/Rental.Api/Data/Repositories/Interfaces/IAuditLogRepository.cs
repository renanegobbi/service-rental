using Rental.Api.Entities.Audit;
using Rental.Core.Data;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add a new audit log
        /// </summary>
        Task Add(AuditLog entrada);
    }
}
