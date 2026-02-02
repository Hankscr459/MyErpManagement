namespace MyErpManagement.Core.Dtos.Products.Response
{
    public class ProductListItemDto
    {
        public Guid Id { get; set; }

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
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// 分類物件
        /// </summary>
        public virtual ProductCategoryViewItemDto? ProductCategory { get; set; }
    }
}
