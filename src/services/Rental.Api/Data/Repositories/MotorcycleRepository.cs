using Microsoft.EntityFrameworkCore;
using Rental.Api.Data;
using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Infrastructure.Repository
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly RentalContext _context;

        public MotorcycleRepository(RentalContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            return await _context.Motorcycles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Motorcycle?> GetByIdAsync(Guid id)
        {
            return await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Motorcycle?> GetByPlateAsync(string plate)
        {
            return await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public void Add(Motorcycle motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
        }

        public void Update(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
        }

        public void Delete(Motorcycle motorcycle)
        {
            _context.Motorcycles.Remove(motorcycle);
        }
    }
}
