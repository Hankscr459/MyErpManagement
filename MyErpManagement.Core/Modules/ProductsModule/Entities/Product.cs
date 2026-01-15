using System.ComponentModel.DataAnnotations.Schema;

namespace MyErpManagement.Core.Modules.ProductsModule.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public Guid ProductCategoryId { get; set; }

        /// <summary>
        /// 編碼
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 商品規格
        /// </summary>
        public string? Specification { get; set; }

        /// <summary>
        /// 進貨單價
        /// </summary>
        public double? PurchasePrice { get; set; }

        /// <summary>
        /// 銷貨單價
        /// </summary>
        public double? SalesPrice { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreatedAt { get; set; }

        // --- 新增導覽屬性 ---
        /// <summary>
        /// 對應的分類實體
        /// </summary>
        [ForeignKey("ProductCategoryId")] // 明確指定外鍵欄位
        public virtual ProductCategory? ProductCategory { get; set; }
    }
}
