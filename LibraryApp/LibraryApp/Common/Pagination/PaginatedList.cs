namespace LibraryApp.Common.Pagination
{
    public class PaginatedList<T>
    {
        private const int _maxPageSize = 20;
        private int _pageSize = 10;
        private int _pageNumber = 1;
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public IEnumerable<T>? Data { get; set; }
    }
}
