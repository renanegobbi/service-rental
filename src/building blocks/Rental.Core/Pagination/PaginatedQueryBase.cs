using FluentValidation;
using Rental.Core.Messages;

namespace Rental.Core.Pagination
{
    public abstract class PaginatedQueryBase<TResponse, TOrderBy> : Query<TResponse>
    {
        public int? PageIndex { get; }
        public int? PageSize { get; }
        public TOrderBy OrderBy { get; }
        public string SortDirection { get; }

        protected PaginatedQueryBase(TOrderBy orderBy, 
            string sortDirection = "ASC", 
            int? pageIndex = null, 
            int? pageSize = null)
        {
            OrderBy = orderBy;
            SortDirection = NormalizeDirection(sortDirection ?? "ASC");
            PageIndex = pageIndex.GetValueOrDefault(1);
            PageSize = pageSize.GetValueOrDefault(25);
        }

        public bool ShouldPaginate() => PageIndex.HasValue && PageSize.HasValue;

        private string NormalizeDirection(string direction)
        {
            var clean = direction?.Trim();

            if (string.Equals(direction, "ASC", StringComparison.InvariantCultureIgnoreCase))
                return "ASC";

            if (string.Equals(direction, "DESC", StringComparison.InvariantCultureIgnoreCase))
                return "DESC";

            return "ASC";
        }

        public override bool IsValid()
        {
            var validator = new InlineValidator<PaginatedQueryBase<TResponse, TOrderBy>>();

            validator.RuleFor(q => q.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than zero.");

            validator.RuleFor(q => q.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than zero.");

            validator.RuleFor(q => q.SortDirection)
                .Must(d => string.Equals(d, "ASC", StringComparison.InvariantCultureIgnoreCase) ||
                           string.Equals(d, "DESC", StringComparison.InvariantCultureIgnoreCase))
                .WithMessage("Only 'ASC' or 'DESC' are allowed.");

            ValidationResult = validator.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
