using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.ProductsModule.Enums;

namespace MyErpManagement.Core.Dtos.Products.Request
{
    public class ProductListQueryRequestDto
    {
        public string? CategroyId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public ProductListSortByEnum SortBy { get; set; } = ProductListSortByEnum.createdAt;
        public SortDirEnum SortDir { get; set; } = SortDirEnum.desc;
    }
}
