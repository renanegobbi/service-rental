using Microsoft.EntityFrameworkCore;
using Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories
{
    public class DriverLicenseTypeRepository : IDriverLicenseTypeRepository
    {
        private readonly RentalContext _context;

        public DriverLicenseTypeRepository(RentalContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Tuple<DriverLicenseType[], double>> GetAllAsync(GetAllDriverLicenseTypeQuery query)
        {
            var registros = _context.DriverLicenseTypes.AsNoTracking().AsQueryable();

            if (query.StartDate.HasValue)
                registros = registros.Where(x => x.CreatedAt >= query.StartDate.Value);

            if (query.EndDate.HasValue)
                registros = registros.Where(x => x.CreatedAt <= query.EndDate.Value);

            if (!string.IsNullOrEmpty(query.Code))
                registros = registros.Where(f => EF.Functions.ILike(f.Code, $"%{query.Code}%"));

            if (!string.IsNullOrEmpty(query.Description))
                registros = registros.Where(f => EF.Functions.ILike(f.Description, $"%{query.Description}%"));

            if (query.IsActive.HasValue)
                registros = registros.Where(f => f.IsActive == query.IsActive);

            // Ordenação
            switch (query.OrderBy)
            {
                case DriverLicenseTypeOrderBy.Code:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.Code)
                        : registros.OrderBy(f => f.Code);
                    break;

                case DriverLicenseTypeOrderBy.Description:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.Description)
                        : registros.OrderBy(f => f.Description);
                    break;

                case DriverLicenseTypeOrderBy.IsActive:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.IsActive)
                        : registros.OrderBy(f => f.IsActive);
                    break;

                case DriverLicenseTypeOrderBy.CreatedAt:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.CreatedAt)
                        : registros.OrderBy(f => f.CreatedAt);
                    break;
                default:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.Id)
                        : registros.OrderBy(f => f.Id);
                    break;
            }

            var total = await registros.CountAsync();

            var items = query.ShouldPaginate()
                ? await registros
                    .Skip(((int)query.PageIndex - 1) * (int)query.PageSize)
                    .Take((int)query.PageSize)
                    .ToArrayAsync()
                : await registros.ToArrayAsync();

            return new Tuple<DriverLicenseType[], double>(items, total);
        }

        public async Task<DriverLicenseType?> GetByIdAsync(Guid id)
        {
            return await _context.DriverLicenseTypes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<DriverLicenseType?> GetByCodeAsync(string code)
        {
            return await _context.DriverLicenseTypes
                .FirstOrDefaultAsync(t => t.Code == code);
        }

        public async Task<DriverLicenseType?> GetActiveByCodeAsync(string code)
        {
            return await _context.DriverLicenseTypes
                .FirstOrDefaultAsync(x => x.Code == code && x.IsActive);
        }

        public async Task<DriverLicenseType?> GetInactiveByCodeAsync(string code)
        {
            return await _context.DriverLicenseTypes
                .FirstOrDefaultAsync(x => x.Code == code && !x.IsActive);
        }

        public void Add(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Add(driverLicenseType);
        }

        public void Update(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Update(driverLicenseType);
        }

        public void Remove(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Remove(driverLicenseType);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
