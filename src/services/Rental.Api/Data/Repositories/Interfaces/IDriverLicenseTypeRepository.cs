using Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll;
using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories.Interfaces
{
    public interface IDriverLicenseTypeRepository 
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Tuple<DriverLicenseType[], double>> GetAllAsync(GetAllDriverLicenseTypeQuery query);
        Task<DriverLicenseType?> GetByIdAsync(Guid id);
        Task<DriverLicenseType?> GetByCodeAsync(string code);
        Task<DriverLicenseType?> GetActiveByCodeAsync(string code);
        Task<DriverLicenseType?> GetInactiveByCodeAsync(string code);
        void Add(DriverLicenseType driverLicenseType);
        void Update(DriverLicenseType driverLicenseType);
        void Remove(DriverLicenseType courdriverLicenseTypeier);
    }
}
