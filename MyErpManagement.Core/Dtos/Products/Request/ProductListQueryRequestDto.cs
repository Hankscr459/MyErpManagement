using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.ProductsModule.Enums;
using System.ComponentModel;

namespace MyErpManagement.Core.Dtos.Products.Request
{
    /// <summary>
    /// 商品物件列表查詢參數
    /// </summary>
    public class ProductListQueryRequestDto
    {
        /// <summary>
        /// 分類 Id
        /// </summary>
        public string? CategroyId { get; set; }
        
        /// <summary>
        /// 第幾頁
        /// </summary>
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int Limit { get; set; } = 10;

        /// <summary>
        /// 排序欄位 (預設: createdAt)
        /// </summary>
        [DefaultValue(ProductListSortByEnum.createdAt)]
        public ProductListSortByEnum SortBy { get; set; } = ProductListSortByEnum.createdAt;
        
        /// <summary>
        /// 排序方向 (預設: desc)
        /// </summary>
        [DefaultValue(SortDirEnum.desc)]
        public SortDirEnum SortDir { get; set; } = SortDirEnum.desc;
    }
}
