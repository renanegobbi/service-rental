using Rental.Api.Swagger;
using System;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class DeleteRentalPlanRequest : IExposeInSwagger
    {
        /// <summary>
        /// Unique identifier of the rental plan to be deleted.
        /// </summary>
        public Guid Id { get; set; }
    }
}
