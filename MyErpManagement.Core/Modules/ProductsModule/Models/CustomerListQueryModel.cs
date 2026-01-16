using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.CustomerModule.Enums;

namespace MyErpManagement.Core.Modules.ProductsModule.Models
{
    public class CustomerListQueryModel
    {
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
        public CustomerListSortByEnum SortBy { get; set; } = CustomerListSortByEnum.createdAt;

        /// <summary>
        /// 排序方向 (預設: desc)
        /// </summary>
        public SortDirEnum SortDir { get; set; } = SortDirEnum.desc;

        /// <summary>
        /// 關鍵字查詢
        /// </summary>
        public string? Search { get; set; }
    }
}
