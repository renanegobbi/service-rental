using Microsoft.EntityFrameworkCore;
using Rental.Api.Application.Queries.CourierQueries.GetAll;
using Rental.Api.Configuration;
using Rental.Api.Data.Cache;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Entities;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Api.Data.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly RentalContext _context;
        private readonly ICacheService<Courier> _cache;
        private readonly CacheDefaults _defaults;
        public CourierRepository(
            RentalContext context,
            ICacheService<Courier> cache,
            CacheDefaults defaults)
        {
            _context = context;
            _cache = cache;
            _defaults = defaults;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Courier>> GetAllAsync()
        {
            return await _context.Couriers
                .AsNoTracking()
                .ToListAsync();
        }

        private static string BuildCacheKey(GetAllCourierQuery query)
        {
            var sb = new StringBuilder("Courier:GetAll");

            if (!string.IsNullOrEmpty(query.FullName)) sb.Append($":FullName={query.FullName}");
            if (!string.IsNullOrEmpty(query.Cnpj)) sb.Append($":Cnpj={query.Cnpj}");
            if (!string.IsNullOrEmpty(query.DriverLicenseNumber)) sb.Append($":DriverLicenseNumber={query.DriverLicenseNumber}");
            if (!string.IsNullOrEmpty(query.DriverLicenseType.ToString())) sb.Append($":DriverLicenseType={query.DriverLicenseType}");
            sb.Append($":OrderBy={query.OrderBy}:Sort={query.SortDirection}");
            sb.Append($":Page={query.PageIndex}:Size={query.PageSize}");

            return sb.ToString();
        }

        public async Task<Tuple<Courier[], double>> GetAllAsync(GetAllCourierQuery query)
        {
            var cacheKey = BuildCacheKey(query);

            var items = await _cache.GetByKeyAsync(cacheKey, async () =>
            {
                var records = _context.Couriers.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(query.FullName))
                    records = records.Where(f => EF.Functions.ILike(f.FullName, $"%{query.FullName}%"));

                if (!string.IsNullOrEmpty(query.Cnpj))
                    records = records.Where(f => EF.Functions.ILike(f.Cnpj, $"%{query.Cnpj}%"));

                if (query.BirthDate.HasValue)
                    records = records.Where(x => x.BirthDate == query.BirthDate.Value);

                if (!string.IsNullOrEmpty(query.DriverLicenseNumber))
                    records = records.Where(f => EF.Functions.ILike(f.DriverLicenseNumber, $"%{query.DriverLicenseNumber}%"));

                if (query.DriverLicenseType.HasValue)
                    records = records.Where(f => f.DriverLicenseType == query.DriverLicenseType.Value);

                if (query.CreatedAt.HasValue)
                    records = records.Where(x => x.CreatedAt == query.CreatedAt.Value.UtcDateTime);

                records = query.OrderBy switch
                {
                    CourierOrderBy.FullName => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.FullName)
                        : records.OrderBy(f => f.FullName),

                    CourierOrderBy.Cnpj => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Cnpj)
                        : records.OrderBy(f => f.Cnpj),

                    CourierOrderBy.BirthDate => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.BirthDate)
                        : records.OrderBy(f => f.BirthDate),

                    CourierOrderBy.DriverLicenseNumber => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.DriverLicenseNumber)
                        : records.OrderBy(f => f.DriverLicenseNumber),

                    CourierOrderBy.CreatedAt => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.CreatedAt)
                        : records.OrderBy(f => f.CreatedAt),

                    _ => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Id)
                        : records.OrderBy(f => f.Id),
                };

                var list = await records.ToListAsync();
                return list;
            }, TimeSpan.FromHours(_defaults.DefaultTtl.TotalHours));

            var total = items.Count;

            var paged = query.ShouldPaginate()
                ? items
                    .Skip(((int)query.PageIndex - 1) * (int)query.PageSize)
                    .Take((int)query.PageSize)
                    .ToArray()
                : items.ToArray();

            return new Tuple<Courier[], double>(paged, total);
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

    }
}
