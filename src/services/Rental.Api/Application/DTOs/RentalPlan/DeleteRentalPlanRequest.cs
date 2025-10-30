using System;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class DeleteRentalPlanRequest
    {
        /// <summary>
        /// Unique identifier of the rental plan to be deleted.
        /// </summary>
        public Guid Id { get; set; }
    }
}
