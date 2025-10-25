using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories.Interfaces
{
    public interface ICourierRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Courier>> GetAllAsync();
        Task<Courier?> GetByIdAsync(Guid id);
        Task<Courier?> GetByCnpjAsync(string cnpj);
        Task<Courier?> GetByDriverLicenseNumberAsync(string licenseNumber);

        void Add(Courier courier);
        void Update(Courier courier);
        void Remove(Courier courier);
    }
}
