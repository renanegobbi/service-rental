using Rental.Api.Swagger;
using Rental.Core.Application.Queries.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rental.Api.Application.DTOs.Courier
{
    public class GetAllCourierRequest : IExposeInSwagger
    {
        /// <summary>
        /// Courier full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Courier CNPJ (numbers only).
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Courier birth date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Driver license number.
        /// </summary>
        public string DriverLicenseNumber { get; set; }

        /// <summary>
        /// Driver license type (e.g., A, B, AB).
        /// </summary>
        public Guid? DriverLicenseType { get; set; }

        /// <summary>
        /// Date and time when the courier was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

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
        [EnumDataType(typeof(CourierOrderBy), ErrorMessage = "Invalid sort type.")]
        public CourierOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC or DESC).
        /// </summary>
        [RegularExpression("(?i)^(ASC|DESC)$", ErrorMessage = "Only ASC or DESC are allowed.")]
        public string? SortDirection { get; set; } = "ASC";
    }
}
