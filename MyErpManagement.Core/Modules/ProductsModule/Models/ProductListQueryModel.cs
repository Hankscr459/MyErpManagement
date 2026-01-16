using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.ProductsModule.Enums;

namespace MyErpManagement.Core.Modules.ProductsModule.Models
{
    public class ProductListQueryModel
    {
        public string? CategroyId { get; set; }

        // 排序欄位
        public ProductListSortByEnum SortBy { get; set; } = ProductListSortByEnum.createdAt;

        // asc / desc
        public SortDirEnum SortDir { get; set; } = SortDirEnum.desc;

        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
