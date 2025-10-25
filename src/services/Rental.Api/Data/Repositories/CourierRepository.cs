using Microsoft.EntityFrameworkCore;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly RentalContext _context;

        public CourierRepository(RentalContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Courier>> GetAllAsync()
        {
            return await _context.Couriers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Courier?> GetByIdAsync(Guid id)
        {
            return await _context.Couriers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Courier?> GetByCnpjAsync(string cnpj)
        {
            return await _context.Couriers
                .FirstOrDefaultAsync(c => c.Cnpj == cnpj);
        }

        public async Task<Courier?> GetByDriverLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Couriers
                .FirstOrDefaultAsync(c => c.DriverLicenseNumber == licenseNumber);
        }

        public void Add(Courier courier)
        {
            _context.Couriers.Add(courier);
        }

        public void Update(Courier courier)
        {
            _context.Couriers.Update(courier);
        }

        public void Remove(Courier courier)
        {
            _context.Couriers.Remove(courier);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
