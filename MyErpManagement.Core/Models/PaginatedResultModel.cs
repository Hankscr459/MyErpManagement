namespace MyErpManagement.Core.Models
{
    public class PaginatedResultModel<T>
    {
        public IEnumerable<T> Docs { get; set; } = new List<T>();
        public int TotalDocs { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int? PagingCounter { get; set; }
        public bool HasPrevPage { get; set; }
        public bool HasNextPage { get; set; }
        public int? PrevPage { get; set; }
        public int? NextPage { get; set; }
    }
}
