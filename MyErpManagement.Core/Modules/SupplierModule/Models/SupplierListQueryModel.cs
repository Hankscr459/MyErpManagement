using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.SupplierModule.Enums;

namespace MyErpManagement.Core.Modules.SupplierModule.Models
{
    public class SupplierListQueryModel
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
        public SupplierListSortByEnum SortBy { get; set; } = SupplierListSortByEnum.createdAt;

        /// <summary>
        /// 排序方向 (預設: desc)
        /// </summary>
        public SortDirEnum SortDir { get; set; } = SortDirEnum.desc;

        /// <summary>
        /// 關鍵字查詢
        /// </summary>
        public string? Search { get; set; }

        public IEnumerable<Guid>? SupplierTagIds { get; set; }
    }
}
