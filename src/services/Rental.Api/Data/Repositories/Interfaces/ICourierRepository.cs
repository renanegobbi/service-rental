using Rental.Api.Application.Queries.CourierQueries.GetAll;
using Rental.Api.Application.Queries.MotorcycleQueries.GetAll;
using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories.Interfaces
{
    public interface ICourierRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Tuple<Courier[], double>> GetAllAsync(GetAllCourierQuery query);
        Task<IEnumerable<Courier>> GetAllAsync();
        Task<Courier?> GetByIdAsync(Guid id);
        Task<Courier?> GetByCnpjAsync(string cnpj);
        Task<Courier?> GetByDriverLicenseNumberAsync(string licenseNumber);

        void Add(Courier courier);
        void Update(Courier courier);
        void Remove(Courier courier);
    }
}
