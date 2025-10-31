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

namespace Rental.Api.Infrastructure.Repository
{
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly RentalContext _context;

        public RentalPlanRepository(RentalContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Tuple<RentalPlan[], double>> GetAllAsync(GetAllRentalPlanQuery query)
        {
            var registros = _context.RentalPlans.AsNoTracking().AsQueryable();

            if (query.StartDate.HasValue)
                registros = registros.Where(x => x.CreatedAt >= query.StartDate.Value);

            if (query.EndDate.HasValue)
                registros = registros.Where(x => x.CreatedAt <= query.EndDate.Value);

            if (query.Days.HasValue)
                registros = registros.Where(f => f.Days == query.Days.Value);

            if (!string.IsNullOrEmpty(query.Description))
                registros = registros.Where(f => EF.Functions.ILike(f.Description, $"%{query.Description}%"));

            if (query.MinDailyRate.HasValue)
                registros = registros.Where(f => f.DailyRate >= query.MinDailyRate.Value);

            if (query.MaxDailyRate.HasValue)
                registros = registros.Where(f => f.DailyRate <= query.MaxDailyRate.Value);

            if (query.MinPenaltyPercent.HasValue)
                registros = registros.Where(f => f.PenaltyPercent >= query.MinPenaltyPercent.Value);

            if (query.MaxPenaltyPercent.HasValue)
                registros = registros.Where(f => f.PenaltyPercent <= query.MaxPenaltyPercent.Value);

            switch (query.OrderBy)
            {
                case RentalPlanOrderBy.Days:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.Days)
                        : registros.OrderBy(f => f.Days);
                    break;

                case RentalPlanOrderBy.DailyRate:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.DailyRate)
                        : registros.OrderBy(f => f.DailyRate);
                    break;

                case RentalPlanOrderBy.PenaltyPercent:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.PenaltyPercent)
                        : registros.OrderBy(f => f.PenaltyPercent);
                    break;

                case RentalPlanOrderBy.Description:
                    registros = query.SortDirection == "DESC"
                        ? registros.OrderByDescending(f => f.Description)
                        : registros.OrderBy(f => f.Description);
                    break;

                case RentalPlanOrderBy.CreatedAt:
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

            return new Tuple<RentalPlan[], double>(items, total);
        }


        public async Task<IEnumerable<RentalPlan>> GetAllAsync()
        {
            return await _context.RentalPlans.AsNoTracking().ToListAsync();
        }

        public async Task<RentalPlan?> GetByIdAsync(Guid id)
        {
            return await _context.RentalPlans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Add(RentalPlan plan)
        {
            _context.RentalPlans.Add(plan);
        }

        public void Update(RentalPlan plan)
        {
            _context.RentalPlans.Update(plan);
        }

        public void Delete(RentalPlan plan)
        {
            _context.RentalPlans.Remove(plan);
        }

    }
}
