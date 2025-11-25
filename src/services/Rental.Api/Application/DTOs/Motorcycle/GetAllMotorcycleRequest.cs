using Rental.Api.Swagger;
using Rental.Core.Application.Queries.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace Rental.Api.Application.DTOs.Motorcycle
{
    public class GetAllMotorcycleRequest : IExposeInSwagger
    {
        /// <summary>
        /// Manufacturing year of the motorcycle (must be greater than 2000).
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Motorcycle model (e.g., "Honda CG 160").
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// License plate (e.g., "ABC1D23").
        /// </summary>
        public string Plate { get; set; }

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
        [EnumDataType(typeof(MotorcycleOrderBy), ErrorMessage = "Invalid sort type.")]
        public MotorcycleOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC or DESC).
        /// </summary>
        [RegularExpression("(?i)^(ASC|DESC)$", ErrorMessage = "Only ASC or DESC are allowed.")]
        public string? SortDirection { get; set; } = "ASC";
    }
}
