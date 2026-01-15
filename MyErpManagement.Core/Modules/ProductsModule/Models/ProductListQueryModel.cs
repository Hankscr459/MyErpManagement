namespace MyErpManagement.Core.Modules.ProductsModule.Models
{
    public class ProductListQueryModel
    {
        public string? CategroyId { get; set; }

        // 排序欄位
        public string SortBy { get; set; } = "CreatedAt";   // Name / SalesPrice / CreatedAt

        // asc / desc
        public string SortDir { get; set; } = "desc";  // asc / desc

        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
