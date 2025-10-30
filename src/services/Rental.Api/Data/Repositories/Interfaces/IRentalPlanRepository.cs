using Rental.Api.Application.Queries.RentalPlanQueries.GetAll;
using Rental.Api.Entities;
using Rental.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Api.Infrastructure.Repository
{
    public interface IRentalPlanRepository : IRepository<RentalPlan>
    {
        Task<Tuple<RentalPlan[], double>> GetAllAsync(GetAllRentalPlanQuery query);
        Task<IEnumerable<RentalPlan>> GetAllAsync();
        Task<RentalPlan?> GetByIdAsync(Guid id);
        void Add(RentalPlan plan);
        void Update(RentalPlan plan);
        void Delete(RentalPlan plan);
    }
}
