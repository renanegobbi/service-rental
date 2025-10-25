using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories.Interfaces
{
    public interface IMotorcycleRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task<Motorcycle?> GetByIdAsync(Guid id);
        Task<Motorcycle?> GetByPlateAsync(string plate);

        void Add(Motorcycle motorcycle);
        void Update(Motorcycle motorcycle);
        void Remove(Motorcycle motorcycle);
    }
}
