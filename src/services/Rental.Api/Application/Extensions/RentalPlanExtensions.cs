using Rental.Api.Application.Commands.RentalPlanCommands.Add;
using Rental.Api.Application.DTOs.RentalPlan;
using Rental.Api.Entities;

namespace Rental.Api.Application.Extensions
{
    public static class RentalPlanExtensions
    {
        public static RentalPlan ToRentalPlan(this AddRentalPlanCommand command)
        {
            if (command == null) return null;

            return new RentalPlan(command.Days, command.DailyRate, command.PenaltyPercent, command.Description);
        }

        public static AddRentalPlanResponse ToAddRentalPlanResponse(this RentalPlan rentalPlan)
        {
            if (rentalPlan == null) return null;

            return new AddRentalPlanResponse
            {
                Id = rentalPlan.Id,
                Days = rentalPlan.Days,
                DailyRate = rentalPlan.DailyRate,
                PenaltyPercent = rentalPlan.PenaltyPercent,
                Description = rentalPlan.Description,
                CreatedAt = rentalPlan.CreatedAt
            };
        }

        public static UpdateRentalPlanResponse ToUpdateRentalPlanResponse(this RentalPlan rentalPlan)
        {
            if (rentalPlan == null) return null;

            return new UpdateRentalPlanResponse
            {
                Id = rentalPlan.Id,
                Days = rentalPlan.Days,
                DailyRate = rentalPlan.DailyRate,
                PenaltyPercent = rentalPlan.PenaltyPercent,
                Description = rentalPlan.Description
            };
        }

        public static DeleteRentalPlanResponse ToDeleteRentalPlanResponse(this RentalPlan rentalPlan)
        {
            if (rentalPlan == null) return null;

            return new DeleteRentalPlanResponse
            {
                Id = rentalPlan.Id,
                Days = rentalPlan.Days,
                DailyRate = rentalPlan.DailyRate,
                PenaltyPercent = rentalPlan.PenaltyPercent,
                Description = rentalPlan.Description
            };
        }

    }
}
