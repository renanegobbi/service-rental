using Rental.Core.DomainObjects;
using System;

namespace Rental.Api.Entities
{
    public class RentalPlan : Entity, IAggregateRoot
    {
        public int Days { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal PenaltyPercent { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected RentalPlan() { }

        public RentalPlan(int days, decimal dailyRate, decimal penaltyPercent, string? description)
        {
            Days = days;
            DailyRate = dailyRate;
            PenaltyPercent = penaltyPercent;
            Description = description;
            CreatedAt = DateTime.Now;
        }

        public void Update(decimal dailyRate, decimal penaltyPercent, string? description)
        {
            DailyRate = dailyRate;
            PenaltyPercent = penaltyPercent;
            Description = description;
        }
    }
}
