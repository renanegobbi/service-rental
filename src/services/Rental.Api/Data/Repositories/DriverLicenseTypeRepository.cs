using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rental.Api.Application.Queries.DriverLicenseTypeQueries.GetAll;
using Rental.Api.Configuration;
using Rental.Api.Data.Cache;
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
        private readonly ICacheService<DriverLicenseType> _cache;
        private readonly CacheDefaults _defaults;
        private readonly ILogger<DriverLicenseTypeRepository> _logger;

        public DriverLicenseTypeRepository(RentalContext context,
                                          ICacheService<DriverLicenseType> cache,
                                          CacheDefaults defaults, 
                                          ILogger<DriverLicenseTypeRepository> logger)
        {
            _context = context;
            _cache = cache;
            _defaults = defaults;
            _logger = logger;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Tuple<DriverLicenseType[], double>> GetAllAsync(GetAllDriverLicenseTypeQuery query)
        {
            var cacheKey = BuildCacheKey(query);

            var items = await _cache.GetByKeyAsync(cacheKey, async () =>
            {
                var records = _context.DriverLicenseTypes.AsNoTracking().AsQueryable();

                if (query.StartDate.HasValue)
                    records = records.Where(x => x.CreatedAt >= query.StartDate.Value);

                if (query.EndDate.HasValue)
                    records = records.Where(x => x.CreatedAt <= query.EndDate.Value);

                if (!string.IsNullOrEmpty(query.Code))
                    records = records.Where(f => EF.Functions.ILike(f.Code, $"%{query.Code}%"));

                if (!string.IsNullOrEmpty(query.Description))
                    records = records.Where(f => EF.Functions.ILike(f.Description, $"%{query.Description}%"));

                if (query.IsActive.HasValue)
                    records = records.Where(f => f.IsActive == query.IsActive);

                records = query.OrderBy switch
                {
                    DriverLicenseTypeOrderBy.Code => query.SortDirection == "DESC" ? records.OrderByDescending(f => f.Code) : records.OrderBy(f => f.Code),
                    DriverLicenseTypeOrderBy.Description => query.SortDirection == "DESC" ? records.OrderByDescending(f => f.Description) : records.OrderBy(f => f.Description),
                    DriverLicenseTypeOrderBy.IsActive => query.SortDirection == "DESC" ? records.OrderByDescending(f => f.IsActive) : records.OrderBy(f => f.IsActive),
                    DriverLicenseTypeOrderBy.CreatedAt => query.SortDirection == "DESC" ? records.OrderByDescending(f => f.CreatedAt) : records.OrderBy(f => f.CreatedAt),
                    _ => query.SortDirection == "DESC" ? records.OrderByDescending(f => f.Id) : records.OrderBy(f => f.Id)
                };

                var list = await records.ToListAsync();
                return list;
            }, TimeSpan.FromHours(_defaults.DefaultTtl.TotalHours));

            var total = items.Count;
            var paged = query.ShouldPaginate()
                ? items.Skip(((int)query.PageIndex - 1) * (int)query.PageSize)
                       .Take((int)query.PageSize)
                       .ToArray()
                : items.ToArray();

            return new Tuple<DriverLicenseType[], double>(paged, total);
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

        public async void Add(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Add(driverLicenseType);

            await CacheInvalidationHelper.InvalidateEntityAsync( _cache, nameof(DriverLicenseType));
        }

        public async void Update(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Update(driverLicenseType);

            await CacheInvalidationHelper.InvalidateEntityAsync(_cache, nameof(DriverLicenseType));
        }

        public async void Remove(DriverLicenseType driverLicenseType)
        {
            _context.DriverLicenseTypes.Remove(driverLicenseType);

            await CacheInvalidationHelper.InvalidateEntityAsync(_cache, nameof(DriverLicenseType));
        }

        private static string BuildCacheKey(GetAllDriverLicenseTypeQuery query)
        {
            var key = $"DriverLicenseType:GetAll";

            if (query.StartDate.HasValue) key += $":Start={query.StartDate:yyyyMMdd}";
            if (query.EndDate.HasValue) key += $":End={query.EndDate:yyyyMMdd}";
            if (!string.IsNullOrEmpty(query.Code)) key += $":Code={query.Code}";
            if (!string.IsNullOrEmpty(query.Description)) key += $":Desc={query.Description}";
            if (query.IsActive.HasValue) key += $":Active={query.IsActive}";
            key += $":OrderBy={query.OrderBy}:Sort={query.SortDirection}:Page={query.PageIndex}:Size={query.PageSize}";

            return key;
        }

    }
}
