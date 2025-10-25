using Rental.Core.Application.Queries.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rental.Api.Application.DTOs.DriverLicenseType
{
    public class GetAllDriverLicenseTypeRequest
    {
        /// <summary>
        /// License code (A, B, AB...)
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Description of the license type
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Record status
        /// </summary>
        public bool? IsActive { get; set; }

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
        /// Field used for sorting
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(DriverLicenseTypeOrderBy), ErrorMessage = "Invalid sort type.")]
        public DriverLicenseTypeOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC or DESC)
        /// </summary>
        [RegularExpression("(?i)^(ASC|DESC)$", ErrorMessage = "Only ASC or DESC are allowed.")]
        public string? SortDirection { get; set; } = "ASC";
    }
}

