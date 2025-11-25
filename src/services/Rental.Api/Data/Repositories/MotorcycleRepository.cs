using Microsoft.EntityFrameworkCore;
using Rental.Api.Application.Queries.MotorcycleQueries.GetAll;
using Rental.Api.Configuration;
using Rental.Api.Data;
using Rental.Api.Data.Cache;
using Rental.Api.Entities;
using Rental.Core.Application.Queries.Enums;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Api.Infrastructure.Repository
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly RentalContext _context;
        private readonly ICacheService<Motorcycle> _cache;
        private readonly CacheDefaults _defaults;

        public MotorcycleRepository(
                RentalContext context, 
                ICacheService<Motorcycle> cache, 
                CacheDefaults defaults
            )
        {
            _context = context;
            _cache = cache;
            _defaults = defaults;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            return await _context.Motorcycles
                .AsNoTracking()
                .ToListAsync();
        }

        private static string BuildCacheKey(GetAllMotorcycleQuery query)
        {
            var sb = new StringBuilder("Motorcycle:GetAll");

            if (query.StartDate.HasValue) sb.Append($":Start={query.StartDate:yyyyMMdd}");
            if (query.EndDate.HasValue) sb.Append($":End={query.EndDate:yyyyMMdd}");
            if (!string.IsNullOrEmpty(query.Model)) sb.Append($":Model={query.Model}");
            if (!string.IsNullOrEmpty(query.Plate)) sb.Append($":Plate={query.Plate}");
            if (query.Year.HasValue) sb.Append($":Year={query.Year}");
            sb.Append($":OrderBy={query.OrderBy}:Sort={query.SortDirection}");
            sb.Append($":Page={query.PageIndex}:Size={query.PageSize}");

            return sb.ToString();
        }

        public async Task<Tuple<Motorcycle[], double>> GetAllAsync(GetAllMotorcycleQuery query)
        {
            var cacheKey = BuildCacheKey(query);

            var items = await _cache.GetByKeyAsync(cacheKey, async () =>
            {
                var records = _context.Motorcycles.AsNoTracking().AsQueryable();

                if (query.StartDate.HasValue)
                    records = records.Where(x => x.CreatedAt >= query.StartDate.Value);

                if (query.EndDate.HasValue)
                    records = records.Where(x => x.CreatedAt <= query.EndDate.Value);

                if (query.Year.HasValue)
                    records = records.Where(f => f.Year == query.Year.Value);

                if (!string.IsNullOrEmpty(query.Model))
                    records = records.Where(f => EF.Functions.ILike(f.Model, $"%{query.Model}%"));

                if (!string.IsNullOrEmpty(query.Plate))
                    records = records.Where(f => EF.Functions.ILike(f.Plate, $"%{query.Plate}%"));

                records = query.OrderBy switch
                {
                    MotorcycleOrderBy.Year => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Year)
                        : records.OrderBy(f => f.Year),

                    MotorcycleOrderBy.Model => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Model)
                        : records.OrderBy(f => f.Model),

                    MotorcycleOrderBy.Plate => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Plate)
                        : records.OrderBy(f => f.Plate),

                    MotorcycleOrderBy.CreatedAt => query.SortDirection == "DESC"
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

            return new Tuple<Motorcycle[], double>(paged, total);
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
