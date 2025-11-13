using Rental.Api.Swagger;
using Rental.Core.Application.Queries.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rental.Api.Application.DTOs.RentalPlan
{
    public class GetAllRentalPlanRequest : IExposeInSwagger
    {
        /// <summary>
        /// Number of days for the rental plan (e.g., 7, 15, 30).
        /// </summary>
        public int? Days { get; set; }

        /// <summary>
        /// Optional description filter.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Minimum penalty percentage filter.
        /// </summary>
        public decimal? MinPenaltyPercent { get; set; }

        /// <summary>
        /// Maximum penalty percentage filter.
        /// </summary>
        public decimal? MaxPenaltyPercent { get; set; }

        /// <summary>
        /// Minimum daily rate filter.
        /// </summary>
        public decimal? MinDailyRate { get; set; }

        /// <summary>
        /// Maximum daily rate filter.
        /// </summary>
        public decimal? MaxDailyRate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid start date.")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid end date.")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Page index (starting from 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page index must be greater than zero.")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Number of records per page
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than zero.")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Field used for sorting.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(RentalPlanOrderBy), ErrorMessage = "Invalid sort type.")]
        public RentalPlanOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC or DESC).
        /// </summary>
        [RegularExpression("(?i)^(ASC|DESC)$", ErrorMessage = "Only ASC or DESC are allowed.")]
        public string? SortDirection { get; set; } = "ASC";
    }
}
