using Microsoft.EntityFrameworkCore;
using Rental.Api.Data;
using Rental.Api.Entities;
using Rental.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Core.Application.Queries.Enums;
using System.Linq;
using Rental.Api.Configuration;
using Rental.Api.Data.Cache;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Rental.Api.Infrastructure.Repository
{
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly RentalContext _context;
        private readonly ICacheService<RentalPlan> _cache;
        private readonly CacheDefaults _defaults;
        private readonly ILogger<RentalPlanRepository> _logger;

        public RentalPlanRepository(RentalContext context,
                                    ICacheService<RentalPlan> cache,
                                    CacheDefaults defaults, ILogger<RentalPlanRepository> logger)
        {
            _context = context;
            _cache = cache;
            _defaults = defaults;
            _logger = logger;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Tuple<RentalPlan[], double>> GetAllAsync(GetAllRentalPlanQuery query)
        {
            var cacheKey = BuildCacheKey(query);

            var items = await _cache.GetByKeyAsync(cacheKey, async () =>
            {
                var records = _context.RentalPlans.AsNoTracking().AsQueryable();

                if (query.StartDate.HasValue)
                    records = records.Where(x => x.CreatedAt >= query.StartDate.Value);

                if (query.EndDate.HasValue)
                    records = records.Where(x => x.CreatedAt <= query.EndDate.Value);

                if (query.Days.HasValue)
                    records = records.Where(f => f.Days == query.Days.Value);

                if (!string.IsNullOrEmpty(query.Description))
                    records = records.Where(f => EF.Functions.ILike(f.Description, $"%{query.Description}%"));

                if (query.MinDailyRate.HasValue)
                    records = records.Where(f => f.DailyRate >= query.MinDailyRate.Value);

                if (query.MaxDailyRate.HasValue)
                    records = records.Where(f => f.DailyRate <= query.MaxDailyRate.Value);

                if (query.MinPenaltyPercent.HasValue)
                    records = records.Where(f => f.PenaltyPercent >= query.MinPenaltyPercent.Value);

                if (query.MaxPenaltyPercent.HasValue)
                    records = records.Where(f => f.PenaltyPercent <= query.MaxPenaltyPercent.Value);

                records = query.OrderBy switch
                {
                    RentalPlanOrderBy.Days => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Days)
                        : records.OrderBy(f => f.Days),

                    RentalPlanOrderBy.DailyRate => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.DailyRate)
                        : records.OrderBy(f => f.DailyRate),

                    RentalPlanOrderBy.PenaltyPercent => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.PenaltyPercent)
                        : records.OrderBy(f => f.PenaltyPercent),

                    RentalPlanOrderBy.Description => query.SortDirection == "DESC"
                        ? records.OrderByDescending(f => f.Description)
                        : records.OrderBy(f => f.Description),

                    RentalPlanOrderBy.CreatedAt => query.SortDirection == "DESC"
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

            return new Tuple<RentalPlan[], double>(paged, total);
        }

        public async Task<IEnumerable<RentalPlan>> GetAllAsync()
        {
            return await _cache.GetByKeyAsync("RentalPlan:GetAll", async () =>
            {
                return await _context.RentalPlans.AsNoTracking().ToListAsync();
            }, _defaults.DefaultTtl);
        }

        public async Task<RentalPlan?> GetByIdAsync(Guid id)
        {
            return await _context.RentalPlans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async void Add(RentalPlan rentalPlan)
        {
            _context.RentalPlans.Add(rentalPlan);

            await CacheInvalidationHelper.InvalidateEntityAsync(_cache, nameof(RentalPlan));
        }

        public async void Update(RentalPlan rentalPlan)
        {
            _context.RentalPlans.Update(rentalPlan);

            await CacheInvalidationHelper.InvalidateEntityAsync(_cache, nameof(RentalPlan));
        }

        public async void Delete(RentalPlan rentalPlan)
        {
            _context.RentalPlans.Remove(rentalPlan);

            await CacheInvalidationHelper.InvalidateEntityAsync(_cache, nameof(RentalPlan));
        }

        private static string BuildCacheKey(GetAllRentalPlanQuery query)
        {
            var sb = new StringBuilder("RentalPlan:GetAll");

            if (query.StartDate.HasValue) sb.Append($":Start={query.StartDate:yyyyMMdd}");
            if (query.EndDate.HasValue) sb.Append($":End={query.EndDate:yyyyMMdd}");
            if (query.Days.HasValue) sb.Append($":Days={query.Days}");
            if (!string.IsNullOrEmpty(query.Description)) sb.Append($":Desc={query.Description}");
            if (query.MinDailyRate.HasValue) sb.Append($":RateMin={query.MinDailyRate}");
            if (query.MaxDailyRate.HasValue) sb.Append($":RateMax={query.MaxDailyRate}");
            if (query.MinPenaltyPercent.HasValue) sb.Append($":PenMin={query.MinPenaltyPercent}");
            if (query.MaxPenaltyPercent.HasValue) sb.Append($":PenMax={query.MaxPenaltyPercent}");
            sb.Append($":OrderBy={query.OrderBy}:Sort={query.SortDirection}");
            sb.Append($":Page={query.PageIndex}:Size={query.PageSize}");

            return sb.ToString();
        }
    }
}
