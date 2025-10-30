using Rental.Core.Interfaces;
using System;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class UpdateRentalPlanResponse: IResponse
    {
        public Guid Id { get; set; }
        public int Days { get; set; }
        public decimal DailyRate { get; set; }
        public decimal PenaltyPercent { get; set; }
        public string? Description { get; set; }
    }
}
