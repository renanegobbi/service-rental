namespace Rental.Core.Pagination
{
    public class PagedResult<T>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public string? OrderBy { get; private set; }
        public string? SortDirection { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize,
            string? orderBy = null, string? sortDirection = null)
        {
            Items = items ?? new List<T>();
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
            SortDirection = sortDirection;
            TotalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
        }
    }
}
