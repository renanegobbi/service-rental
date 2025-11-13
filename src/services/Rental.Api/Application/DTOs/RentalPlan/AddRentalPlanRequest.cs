using Rental.Api.Swagger;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class AddRentalPlanRequest : IExposeInSwagger
    {
        /// <summary>
        /// Number of days in the plan (e.g., 7, 15, 30).
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Daily rental rate (e.g., 49.90).
        /// </summary>
        public decimal DailyRate { get; set; }

        /// <summary>
        /// Penalty percentage for early return (e.g., 20.0).
        /// </summary>
        public decimal PenaltyPercent { get; set; }

        /// <summary>
        /// Plan description (optional).
        /// </summary>
        public string? Description { get; set; }
    }
}
