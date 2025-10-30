using System;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class UpdateRentalPlanRequest
    {
        /// <summary>
        /// Unique identifier of the rental plan to be updated.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Daily rental rate (e.g., 49.90).
        /// </summary>
        public decimal DailyRate { get; set; }

        /// <summary>
        /// Penalty percentage for early returns (e.g., 20.0).
        /// </summary>
        public decimal PenaltyPercent { get; set; }

        /// <summary>
        /// Optional description for the plan.
        /// </summary>
        public string? Description { get; set; }
    }
}
