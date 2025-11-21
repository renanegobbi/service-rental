using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Infrastructure.Repository
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task<Motorcycle?> GetByIdAsync(Guid id);
        Task<Motorcycle?> GetByPlateAsync(string plate);
        void Add(Motorcycle motorcycle);
        void Update(Motorcycle motorcycle);
        void Delete(Motorcycle motorcycle);
    }
}
